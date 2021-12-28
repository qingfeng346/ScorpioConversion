public class ReaderManager : BaseManager<IReader> {
    public static ReaderManager Instance { get; } = new ReaderManager();
    protected override string Name => "Reader";
}
