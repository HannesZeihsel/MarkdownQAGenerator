# Set the base image as the .NET 5.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
COPY . ./
RUN dotnet publish ./MarkdownQAGenerator/MarkdownQAGenerator.csproj -c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Hannes Zeihsel"
LABEL repository="https://github.com/HannesZeihsel/MarkdownQAGenerator"
LABEL homepage="https://github.com/HannesZeihsel/MarkdownQAGenerator"

# Label as GitHub action
LABEL com.github.actions.name="MarkdownQAGenerator"
LABEL com.github.actions.description="A GitHubAction that uses a markdown file to generate question answer cards for Anki via AnkiJson"
LABEL com.github.actions.icon="refresh-cw"
LABEL com.github.actions.color="blue"

# Relayer the .NET SDK, anew with the build output
# FROM mcr.microsoft.com/dotnet/sdk:5.0
FROM mcr.microsoft.com/dotnet/runtime:5.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/MarkdownQAGenerator.dll" ]