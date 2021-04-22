FROM mcr.microsoft.com/dotnet/core/aspnet:5.0
RUN mkdir -p /app
COPY . /app
WORKDIR /app
ENV TZ=Asia/Shanghai
EXPOSE 80
ENTRYPOINT ["dotnet","Dashboard.dll"]