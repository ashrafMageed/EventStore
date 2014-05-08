using System;
using System.Threading;
using EventStore.Core.Messaging;
using EventStore.Projections.Core.Services;
using EventStore.Projections.Core.Services.Processing;

namespace EventStore.Projections.Core.Messages
{
    public static class CoreProjectionStatusMessage
    {
        public class CoreProjectionStatusMessageBase : CoreProjectionManagementMessageBase
        {

            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            protected CoreProjectionStatusMessageBase(Guid projectionId)
                : base(projectionId)
            {
            }
        }

        public class Started : CoreProjectionStatusMessageBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            public Started(Guid projectionId)
                : base(projectionId)
            {
            }
        }

        public class Faulted : CoreProjectionStatusMessageBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            private readonly string _faultedReason;

            public Faulted(Guid projectionId, string faultedReason)
                : base(projectionId)
            {
                _faultedReason = faultedReason;
            }

            public string FaultedReason
            {
                get { return _faultedReason; }
            }
        }

        public abstract class DataReportBase : CoreProjectionStatusMessageBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            private readonly Guid _correlationId;
            private readonly string _partition;
            private readonly CheckpointTag _position;

            protected DataReportBase(Guid correlationId, Guid projectionId, string partition, CheckpointTag position)
                : base(projectionId)
            {
                _correlationId = correlationId;
                _partition = partition;
                _position = position;
            }

            public string Partition
            {
                get { return _partition; }
            }

            public Guid CorrelationId
            {
                get { return _correlationId; }
            }

            public CheckpointTag Position
            {
                get { return _position; }
            }
        }

        public class StateReport : DataReportBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            private readonly byte[] _state;

            public StateReport(
                Guid correlationId,
                Guid projectionId,
                string partition,
                byte[] state,
                CheckpointTag position)
                : base(correlationId, projectionId, partition, position)
            {
                _state = state;
            }

            public byte[] State
            {
                get { return _state; }
            }

        }

        public class ResultReport : DataReportBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            private readonly byte[] _result;

            public ResultReport(
                Guid correlationId,
                Guid projectionId,
                string partition,
                byte[] result,
                CheckpointTag position)
                : base(correlationId, projectionId, partition, position)
            {
                _result = result;
            }

            public byte[] Result
            {
                get { return _result; }
            }

        }

        public class StatisticsReport : CoreProjectionStatusMessageBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            private readonly ProjectionStatistics _statistics;
            private readonly int _sequentialNumber;

            public StatisticsReport(Guid projectionId, ProjectionStatistics statistics, int sequentialNumber)
                : base(projectionId)
            {
                _statistics = statistics;
                _sequentialNumber = sequentialNumber;
            }

            public ProjectionStatistics Statistics
            {
                get { return _statistics; }
            }

            public int SequentialNumber
            {
                get { return _sequentialNumber; }
            }
        }

        public class Prepared : CoreProjectionStatusMessageBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            private readonly ProjectionSourceDefinition _sourceDefinition;

            public Prepared(Guid projectionId, ProjectionSourceDefinition sourceDefinition)
                : base(projectionId)
            {
                _sourceDefinition = sourceDefinition;
            }

            public ProjectionSourceDefinition SourceDefinition
            {
                get { return _sourceDefinition; }
            }
        }

        public class ProjectionWorkerStarted : Message
        {
            private readonly Guid _workerId;
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            public ProjectionWorkerStarted(Guid workerId)
            {
                _workerId = workerId;
            }

            public Guid WorkerId
            {
                get { return _workerId; }
            }
        }

        public class Stopped : CoreProjectionStatusMessageBase
        {
            private static readonly int TypeId = Interlocked.Increment(ref NextMsgId);

            public override int MsgTypeId
            {
                get { return TypeId; }
            }

            private readonly bool _completed;

            public Stopped(Guid projectionId, bool completed)
                : base(projectionId)
            {
                _completed = completed;
            }

            public bool Completed
            {
                get { return _completed; }
            }
        }
    }
}
