# Define paths
$composeFile = "./compose.yml"
$outputDir = "../output"
$entityResults = "$outputDir/EntityBenchmarkResults.csv"
$dapperResults = "$outputDir/DapperBenchmarkResults.csv"
$combinedResults = "$outputDir/CombinedBenchmarkResults.csv"

# Ensure output directory exists
if (!(Test-Path -Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir
}

# Start Docker Compose
Write-Host "Starting Docker Compose..."
docker-compose -f $composeFile up -d --build

# Wait for services to start
Write-Host "Waiting for services to initialize..."
Start-Sleep -Seconds 30

# Run EntityBenchmark
Write-Host "Running EntityBenchmark..."
docker exec entity_benchmark dotnet EntityBenchmark.dll
docker cp entity_benchmark:/app/output/BenchmarkResults.csv $entityResults

# Run DapperBenchmark
Write-Host "Running DapperBenchmark..."
docker exec dapper_benchmark dotnet DapperBenchmark.dll
docker cp dapper_benchmark:/app/output/BenchmarkResults.csv $dapperResults

# Combine results
Write-Host "Combining results into a single CSV..."
$entityData = Import-Csv -Path $entityResults | ForEach-Object { $_ | Add-Member -MemberType NoteProperty -Name "Source" -Value "EntityFramework" -PassThru }
$dapperData = Import-Csv -Path $dapperResults | ForEach-Object { $_ | Add-Member -MemberType NoteProperty -Name "Source" -Value "Dapper" -PassThru }
$combinedData = $entityData + $dapperData
$combinedData | Export-Csv -Path $combinedResults -NoTypeInformation

# Stop Docker Compose
Write-Host "Stopping Docker Compose..."
docker-compose -f $composeFile down

Write-Host "Benchmarking complete. Combined results saved to: $combinedResults"
