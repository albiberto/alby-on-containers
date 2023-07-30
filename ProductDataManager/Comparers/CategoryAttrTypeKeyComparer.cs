namespace ProductDataManager.Comparers;

using Infrastructure.Domain;

public class CategoryAttrTypeComparer
{
    public class Update: CategoryAttrTypeComparer, IEqualityComparer<CategoryAttrType>
    {
        public bool Equals(CategoryAttrType x, CategoryAttrType y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            if (!x.CategoryId.Equals(y.CategoryId)) return false;

            // Compare the Type object and its properties
            if (!x.Type.Id.Equals(y.Type.Id)) return false;
            if (x.Type.Name != y.Type.Name) return false;
            if (x.Type.Description != y.Type.Description) return false;

            return true;
        }

        public int GetHashCode(CategoryAttrType obj)
        {
            int hashCategoryId = obj.CategoryId.GetHashCode();
            int hashTypeId = obj.Type.Id.GetHashCode();
            int hashTypeName = obj.Type.Name?.GetHashCode() ?? 0;
            int hashTypeDescription = obj.Type.Description?.GetHashCode() ?? 0;

            return hashCategoryId ^ hashTypeId ^ hashTypeName ^ hashTypeDescription;
        }
    }
    
    public class Delete: CategoryAttrTypeComparer, IEqualityComparer<CategoryAttrType>
    {
        public bool Equals(CategoryAttrType x, CategoryAttrType y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.CategoryId.Equals(y.CategoryId);
        }

        public int GetHashCode(CategoryAttrType obj)
        {
            return obj.CategoryId.GetHashCode();
        }
    }
}
