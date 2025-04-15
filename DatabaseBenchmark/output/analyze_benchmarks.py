import pandas as pd
import scipy.stats as stats
import os
from openpyxl import Workbook
from openpyxl.utils.dataframe import dataframe_to_rows
from openpyxl.chart import BarChart, Reference
from openpyxl.styles import Font

# Set working directory and locate file
script_dir = os.path.dirname(os.path.abspath(__file__))
file_path = os.path.join(script_dir, "BenchmarkResults.xlsx")

# Load the sheets
dapper_df = pd.read_excel(file_path, sheet_name="DapperResults")
entity_df = pd.read_excel(file_path, sheet_name="EntityResults")

# Add framework labels
dapper_df["Framework"] = "Dapper"
entity_df["Framework"] = "Entity"

# Combine data
combined_df = pd.concat([dapper_df, entity_df], ignore_index=True)

# Perform ANOVA for each method
anova_results = {}
for method in combined_df["MethodName"].unique():
    method_data = combined_df[combined_df["MethodName"] == method]
    dapper_times = method_data[method_data["Framework"] == "Dapper"]["Duration(ms)"]
    entity_times = method_data[method_data["Framework"] == "Entity"]["Duration(ms)"]
    
    f_val, p_val = stats.f_oneway(dapper_times, entity_times)
    anova_results[method] = {"F-Value": f_val, "P-Value": p_val}

# Create ANOVA DataFrame
anova_df = pd.DataFrame.from_dict(anova_results, orient='index')
anova_df.index.name = "Method"
anova_df.reset_index(inplace=True)

# Save to new Excel file with charts
output_path = os.path.join(script_dir, "BenchmarkAnalysisOutput.xlsx")
wb = Workbook()

# Create ANOVA Results sheet
ws_anova = wb.active
ws_anova.title = "ANOVA Results"

for r in dataframe_to_rows(anova_df, index=False, header=True):
    ws_anova.append(r)

# Add a bar chart (F-Values)
chart = BarChart()
chart.title = "ANOVA F-Values per Method"
chart.y_axis.title = "F-Value"
chart.x_axis.title = "Method"

data = Reference(ws_anova, min_col=2, min_row=1, max_row=len(anova_df)+1)
cats = Reference(ws_anova, min_col=1, min_row=2, max_row=len(anova_df)+1)
chart.add_data(data, titles_from_data=True)
chart.set_categories(cats)

ws_anova.add_chart(chart, "E2")

# Optionally add combined data to another sheet
ws_data = wb.create_sheet(title="Combined Data")
for r in dataframe_to_rows(combined_df, index=False, header=True):
    ws_data.append(r)

# Style the headers
for cell in ws_anova[1]:
    cell.font = Font(bold=True)

for cell in ws_data[1]:
    cell.font = Font(bold=True)

# Save the workbook
wb.save(output_path)
print(f"Excel file with analysis and charts saved to:\n{output_path}")
