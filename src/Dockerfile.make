FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ENV DEBIAN_FRONTEND noninteractive

RUN apt-get -y update && \
  apt-get -y upgrade && \
  apt-get --no-install-recommends -y install \
  g++ python3 dpkg-dev git doxygen tclsh pkg-config cmake libssl-dev build-essential ca-certificates dos2unix graphviz curl locales \
  pcscd libpcsclite-dev libcppunit-dev libcurl4 libcurl4-openssl-dev && \
  apt-get -y clean && \
  sed -i '/en_US.UTF-8/s/^# //g' /etc/locale.gen && locale-gen

ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8

#ENV  LD_LIBRARY_PATH="$LD_LIBRARY_PATH:/usr/local/lib"

# SRT
RUN git clone --depth 1 https://github.com/Haivision/srt.git
WORKDIR srt
RUN ./configure --prefix=/usr && make -j8 && make install && ldconfig




# TSDUCK
# ALTDEVROOT : alternative root for development tools and library; can be the Homebrew root or "/usr" if no other value is suitable
ENV ALTDEVROOT /usr/local

# SYSPREFIX : installation prefix; if not already set on input, it is definedas /usr on Linux and HomeBrew root on macO
ENV SYSPREFIX /usr/local

RUN git clone --depth 1 https://github.com/tsduck/tsduck.git
WORKDIR tsduck
RUN scripts/install-prerequisites.sh
RUN make -j10 NOPCSC=1 NODEKTEC=1 NOHIDES=1 NOVATEK=1 NOEDITLINE=1 NOTEST=1 SYSPREFIX=/usr/local ALTDEVROOT=/usr/local
#RUN make install NOPCSC=1 NODEKTEC=1 NOHIDES=1 NOVATEK=1 NOEDITLINE=1 NOTEST=1 SYSPREFIX=/usr/local

# APP
WORKDIR /src
COPY ./src .
WORKDIR /src/TSDuckEngine
RUN dotnet restore -p:TargetFramework=net7.0


FROM build AS publish
RUN dotnet publish --output /app/publish -c Release /p:UseAppHost=false --no-restore "./TSDuckEngine.csproj"

FROM base AS final
ENV TZ Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENV LD_LIBRARY_PATH /lib:/usr/lib:/usr/local/lib


RUN DEBIAN_FRONTEND=noninteractive \
  apt-get -y update && \
  apt-get -y upgrade && \
  apt-get --no-install-recommends -y install \
  ca-certificates libcurl4 && \
  apt-get -y clean



COPY --from=build /usr/local/bin/ /usr/local/bin/
COPY --from=build /usr/local/lib/libsrt.* /usr/local/lib/
#COPY --from=build /usr/local/lib/libsrt.so.1.5.2 /usr/local/lib/
#COPY --from=build /usr/local/lib/libsrt.so.1.5 /usr/local/lib/
#COPY --from=build /usr/local/lib/libsrt.so /usr/local/lib/
#COPY --from=build /usr/local/lib/libsrt.a /usr/local/lib/
#ln -s existing_source_file optional_symbolic_link
#COPY --from=builder /usr/lib/x86_64-linux-gnu/libsrt.* /usr/lib/x86_64-linux-gnu/

COPY --from=build /usr/local/lib/libtsduck.a /usr/local/lib/
COPY --from=build /usr/local/lib/libtsduck.so /usr/local/lib/
COPY --from=build /usr/local/lib/tsduck /usr/local/lib/

COPY --from=build /usr/local/share/tsduck /usr/local/share/tsduck
COPY --from=build /usr/local/etc/security/console.perms.d/80-tsduck.perms /usr/local/etc/security/console.perms.d/
COPY --from=build /usr/local/etc/udev/rules.d/80-tsduck.rules /usr/local/etc/udev/rules.d/

RUN 

RUN mkdir -p /opt/TSDuckEngine/bin
RUN mkdir -p /opt/TSDuckEngine/etc/TSDuckEngine
WORKDIR /opt/TSDuckEngine/bin
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSDuckEngine.dll"]