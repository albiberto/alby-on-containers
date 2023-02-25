namespace AlbyOnContainers.ProductDataManager.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

public class ExportProductController : ExportController
{
    private readonly CategoryService _service;

    public ExportProductController(CategoryService service) => _service = service;

    [HttpGet("/export/Product/categories/csv")]
    [HttpGet("/export/Product/categories/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCategoriesToCsv(string fileName = null) => ToCSV(ApplyQuery(await _service.GetCategories(), Request.Query), fileName);

    [HttpGet("/export/Product/categories/excel")]
    [HttpGet("/export/Product/categories/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCategoriesToExcel(string fileName = null) => ToExcel(ApplyQuery(await _service.GetCategories(), Request.Query), fileName);
}