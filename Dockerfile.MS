#######################################################
# Step 1: Build the application in a container        #
#######################################################
# Download the official ASP.NET Core SDK image
# to build the project while creating the docker image

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env

ARG SONAR_PROJECT_KEY=yifbackendtkndocker
ARG SONAR_HOST_URL=http://20.67.117.174:9000
ARG SONAR_TOKEN=f05f3118d3403778fd233b5009f3902fadf4460f

WORKDIR /app

# Open ports
EXPOSE 80
EXPOSE 443

# Install Sonar Scanner, Coverlet and Java (required for Sonar Scanner)
RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console --version 1.7.2
ENV PATH="$PATH:/root/.dotnet/tools"

# Restore NuGet packages
COPY . .
RUN dotnet restore

# Start Sonar Scanner
RUN dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /v:1.0 \
  /d:sonar.scm.disabled=true \
  /d:sonar.exclusions="**/SeederDB.cs,**/Migrations/**" \
  /d:sonar.cs.opencover.reportsPaths=YIF_XUnitTests/coverage.opencover.xml \
  /d:sonar.coverage.exclusions="YIF.Core.Data/**,**Test*.cs,**DTO.cs,**Model.cs" \
  /d:sonar.cs.vstest.reportsPaths=YIF_XUnitTests/TestResults/*.trx

# Build and test the application
RUN dotnet publish -c Release -o out

# Run dotnet tests
RUN dotnet test YIF_XUnitTests/YIF_XUnitTests.csproj \
	/p:CollectCoverage=true \
	/p:CoverletOutputFormat=opencover --logger trx

# End Sonar Scanner
RUN dotnet sonarscanner end /d:sonar.login="$SONAR_TOKEN"

#######################################################
# Step 2: Run the build outcome in a container        #
#######################################################
# Download the official ASP.NET Core Runtime image
# to run the compiled application

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app

# Copy the build output from the SDK image
COPY --from=build-env /app/out .

# Start the application
ENTRYPOINT ["dotnet", "YIF_Backend.dll"]