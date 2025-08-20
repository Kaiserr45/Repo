###############################################################################
 
FROM base-docker-local.artifactory.corp.ingos.ru/dotnet/sdk:8.0-alpine-ingo AS build
 
ARG Version
ENV NUGET_XMLDOC_MODE=none
 
WORKDIR /src
 
COPY . .
 
RUN dotnet restore --source "https://artifactory.corp.ingos.ru/api/nuget/common-nuget" && \
    dotnet build --no-restore -c Release -p:Version=${Version}
 
###############################################################################
 
FROM build AS test
 
ENTRYPOINT ["/bin/sh", "-c", "(dotnet test /src/tests/IngoX.Client.Bff.Tests/IngoX.Client.Bff.Tests.csproj --no-restore --no-build -c Release -l:trx --collect:\"XPlat Code Coverage\" --results-directory /TestResults && chmod -R 777 /TestResults) || (chmod -R 777 /TestResults && exit 1)" ]
 
###############################################################################
 
FROM build AS publish
 
RUN dotnet publish /src/src/IngoX.Client.Bff --no-restore --no-build -c Release -o /app
 
###############################################################################
 
FROM base-docker-local.artifactory.corp.ingos.ru/dotnet/aspnet:8.0-alpine-ingo AS final

ARG Version
 
EXPOSE 9001
 
ENV ASPNETCORE_URLS="http://*:9001" \
    Kestrel__Endpoints__Http__Url="http://*:9001" \
	IngosApplication__ReleaseId="${Version}" \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
 
WORKDIR /app
 
COPY --chown=app:root --from=publish /app .
 
ENTRYPOINT ["dotnet", "IngoX.Client.Bff.dll"]
 
###############################################################################