public class ToDo
{
    public enum PiorityExplicit:int  { Blocking=0, Major=1, Minor=2, WhenHaveTime=3 }
    public class Later : System.NotImplementedException
    {
        private Piority m_piority;
        public Piority GetPiorityType() { return m_piority; }
        public Later(Piority piority)
        {
            m_piority = piority;

        }
        public Later(PiorityExplicit piority)
        {
            m_piority = (Piority)((int)piority);

        }
        public Later(Piority piority, string message) : base(piority.ToString() + " :" + message)
        {
            m_piority = piority;
        }
        public Later(PiorityExplicit piority, string message) : base(piority.ToString() + " :" + message)
        {
            m_piority = (Piority) ((int) piority);
        }

    }
}