dotnet restore src/**/project.json
dotnet build --version-suffix $CI_BUILD_ID src/**/project.json

# TODO: run tests
