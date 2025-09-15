using newProject.Domain.Common;

namespace SocialHub.Domain.Users.ValueObjects
{
    public sealed class FollowRequestId : ValueObject
    {
        public Guid Value { get; private set; }

        private FollowRequestId() { }

        private FollowRequestId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("FollowRequestId cannot be empty", nameof(value));
            }

            Value = value;
        }

        public static FollowRequestId Create(Guid value)
        {
            return new FollowRequestId(value);
        }

        public static FollowRequestId Create()
        {
            return new FollowRequestId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
