// using Microsoft.AspNetCore.Components;
// using MudBlazor;
// using ProductDataManager.Components.Pages.Products.Model;
// using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
// using ProductDataManager.Domain.Aggregates.ProductAggregate;
//
// namespace ProductDataManager.Components.Pages.Products;
//
// public partial class Products : ComponentBase
// {
//     AggregatesModel Model { get; set; } = new();
//     
//     protected override async Task OnInitializedAsync() => Model = new(await ProductRepository.GetAllAsync(), await CategoryRepository.GetAllAsync());
//
//     protected override void OnAfterRender(bool firstRender)
//     
//     {
//         if (firstRender) registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
//     }
//
//     async Task AddProductAsync()
//     {
//         try
//         {
//             var entity = await ProductRepository.AddAsync();
//             Model.Add(entity.Id!.Value);
//             
//             Snackbar.Add("Type tracked for insertion", Severity.Info);
//         }
//         catch(Exception e)
//         {
//             Logger.LogError(e, "Error while adding description");
//             Snackbar.Add("Error while adding description", Severity.Error);
//         }
//     }
//
//     async Task UpdateProductAsync(AggregateModel aggregate)
//     {
//         try
//         {
//             if (aggregate.Product.IsDirty)
//             {
//                 await ProductRepository.UpdateAsync(aggregate.Product.Id, aggregate.Product.Name, aggregate.Product.Description, aggregate.Product.CategoryId);
//                 Model.Modified(aggregate);
//             }
//             else await ClearAsync(aggregate);
//             
//             if(aggregate.Product.Status.IsAdded) Snackbar.Add("Type tracked for update", Severity.Info);
//         }
//         catch(Exception e)
//         {
//             Logger.LogError(e, "Error while updating description");
//             Snackbar.Add("Error while updating description", Severity.Error);
//         }
//     }
//
//     async Task DeleteProductAsync(AggregateModel aggregate)
//     {
//         try
//         {
//             await ProductRepository.DeleteAsync(aggregate.Product.Id);
//             Model.Delete(aggregate);    
//             
//             if(!aggregate.Status.IsAdded) Snackbar.Add("Type tracked for deletion", Severity.Info);
//         }
//         catch (Exception e)
//         {
//             Logger.LogError(e, "Error while deleting description");
//             Snackbar.Add("Error while deleting description", Severity.Error);
//         }
//     }
//     
//     async Task ClearAsync(AggregateModel aggregate)
//     {
//         try
//         {
//             await ProductRepository.Clear<Product>(aggregate.Product.Id);
//             aggregate.Product.Clear();
//         }
//         catch (Exception e)
//         {
//             Logger.LogError(e, "Error while clearing description");
//             Snackbar.Add("Error while clearing description", Severity.Error);
//         }
//     }
//
//     async Task SaveAsync()
//     {
//         try
//         {
//             await ProductRepository.UnitOfWork.SaveChangesAsync();
//             Model.Save();
//             
//             Snackbar.Add("Changes Saved!", Severity.Success);
//         }
//         catch (Exception e)
//         {
//             Logger.LogError(e, "Error while deleting category");
//             Snackbar.Add("Error while deleting category", Severity.Error);
//         }
//     }
//
//     public bool DisableSave => !ProductRepository.HasChanges || !Model.IsValid;
//     public bool DisableClearAll => !ProductRepository.HasChanges;
//
//     void ClearAll()
//     {
//         try
//         {
//             ProductRepository.Clear();
//             Model.Clear();
//         }
//         catch (Exception e)
//         {
//             Logger.LogError(e, "Error while clearing all descriptions");
//             Snackbar.Add("Error while clearing all descriptions", Severity.Error);
//         }
//     }
// }