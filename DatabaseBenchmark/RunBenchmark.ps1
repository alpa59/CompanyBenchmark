# Define paths
$outputDir = "./output"
$csvFile1 = Join-Path $outputDir "DapperBenchmarkResults.csv"
$csvFile2 = Join-Path $outputDir "EntityBenchmarkResults.csv"
$excelFile = Join-Path $outputDir "BenchmarkResults.xlsx"

# Ensure the ImportExcel module is available
if (-not (Get-Module -ListAvailable -Name ImportExcel)) {
    Write-Host "The ImportExcel module is not installed. Installing..."
    Install-Module -Name ImportExcel -Scope CurrentUser -Force
}

# Check if both CSV files exist
if (!(Test-Path $csvFile1) -or !(Test-Path $csvFile2)) {
    throw "One or both CSV files do not exist: $csvFile1, $csvFile2"
}

# Import CSV files
Write-Host "Importing CSV files..."
$dapperData = Import-Csv -Path $csvFile1
$entityData = Import-Csv -Path $csvFile2

# Export to Excel with separate sheets
Write-Host "Exporting data to Excel..."
$dapperData | Export-Excel -Path $excelFile -WorksheetName "DapperResults" -NoClobber
$entityData | Export-Excel -Path $excelFile -WorksheetName "EntityResults" -NoClobber

Write-Host "Benchmark results saved to Excel file at: $excelFile"
