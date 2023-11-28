FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /Portfolio.Backend



# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore 
# Clean the solution
RUN dotnet clean
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /Portfolio.Backend
COPY --from=build-env /Portfolio.Backend/out .
ENTRYPOINT ["dotnet", "Portfolio.Backend.Application.dll"]
