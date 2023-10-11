namespace Repro_Cosmos.Model
{
    public class DataType
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        
        public IList<NestedData> Children { get; set; }
    }

}
