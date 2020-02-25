using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Models.Models
{
    public class Procedure
    {
        public string EventType { get; set; }
        public ProcedureStateChangeEvent ProcedureStateChangeEvent { get; set; }
    }
    public class ProcedureStateChangeEvent
    { 
        public string TaskId { get; set; }
        public string Status { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public MetaData MetaData { get; set; }
        public List<MediaProcessResultSet> MediaProcessResultSet { get; set; }
        public int TasksPriority { get; set; }
        public string TasksNotifyMode { get; set; }
    }
    public class MetaData 
    { 
        public int AudioDuration { get; set; }
        public AudioStreamSet AudioStreamSet { get; set; }
        public int Bitrate { get; set; }
        public string Container { get; set; }
        public int Duration { get; set; }
        public int Height { get; set; }
        public int Rotate { get; set; }
        public int Size { get; set; }
        public int VideoDuration { get; set; }
        public VideoStreamSet VideoStreamSet { get; set; }
        public int Width { get; set; }
    }
    public class AudioStreamSet 
    { 
        public int Bitrate { get; set; }
        public string Codec { get; set; }
        public int SamplingRate { get; set; }
    }
    public class VideoStreamSet 
    {
        public int Bitrate { get; set; }
        public string Codec { get; set; }
        public int Fps { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
    public class MediaProcessResultSet 
    { 
        public string Type { get; set; }
        public AdaptiveDynamicStreamingTask AdaptiveDynamicStreamingTask { get; set; }
    }
    public class AdaptiveDynamicStreamingTask 
    { 
        public string Status { get; set; }
        public int ErrCode { get; set; }
        public string Message { get; set; }
        public Input Input { get; set; }
        public Output Output { get; set; }
    }
    public class Input 
    { 
        public int Definition { get; set; }
    }
    public class Output 
    { 
        public int Definition { get; set; }
        public string Package { get; set; }
        public string DrmType { get; set; }
        public string Url { get; set; }
    }
}
