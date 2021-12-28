public class WriterManager : BaseManager<IWriter> {
    public static WriterManager Instance { get; } = new WriterManager();
    protected override string Name => "Writer";
}
