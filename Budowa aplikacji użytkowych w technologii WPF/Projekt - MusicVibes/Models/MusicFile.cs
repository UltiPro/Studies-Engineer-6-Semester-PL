namespace MusicVibes.Models;

public class MusicFile
{
    public int FileId { get; }
    public string FilePath { get; }
    public string FileName { get; }
    public double FileDuration { get; }
    public string FileDurationString { get; }
    public MusicFile(int FileId, string FilePath, string FileName, double FileDuration, string FileDurationString)
    {
        this.FileId = FileId;
        this.FilePath = FilePath;
        this.FileName = FileName;
        this.FileDuration = FileDuration;
        this.FileDurationString = FileDurationString;
    }
}