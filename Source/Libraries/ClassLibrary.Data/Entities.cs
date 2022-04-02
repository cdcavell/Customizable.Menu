namespace ClassLibrary.Data
{
    public enum EntityTypes
    {
        None = 0,
        Menu = 1,
        Site = 2,
        Url = 3
    }

    public class Entities
    {
        #region static methods

        public static List<KeyValuePair<int, string>> GetEnumList()
        {
            List<KeyValuePair<int, string>> enumList = new();
            foreach (var item in Enum.GetValues(typeof(EntityTypes)))
                enumList.Add(new KeyValuePair<int, string>((int)item, item.ToString() ?? string.Empty));

            return enumList;
        }

    #endregion
}
}
