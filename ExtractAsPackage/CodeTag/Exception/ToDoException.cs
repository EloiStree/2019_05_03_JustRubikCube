using System;

public class ToDo
{
    public enum PiorityExplicit:int  { Blocking=0, Major=1, Minor=2, WhenHaveTime=3 }
    public class LaterException : System.NotImplementedException
    {
        private Piority m_piority;
        public Piority GetPiorityType() { return m_piority; }
        public LaterException(Piority piority)
        {
            m_piority = piority;

        }
        public LaterException(PiorityExplicit piority)
        {
            m_piority = (Piority)((int)piority);

        }
        public LaterException(Piority piority, string message) : base(piority.ToString() + " :" + message)
        {
            m_piority = piority;
        }
        public LaterException(PiorityExplicit piority, string message) : base(piority.ToString() + " :" + message)
        {
            m_piority = (Piority) ((int) piority);
        }




    }
    public static LaterException Later(Piority piority)
    {
        return new LaterException(piority);

    }
    public static LaterException Later(PiorityExplicit piority)
    {
        return new LaterException((Piority)((int)piority));
    }
    public static LaterException Later(Piority piority, string message)
    {
        return new LaterException(piority, message);
    }
    public static LaterException Later(PiorityExplicit piority, string message)
    {
        return new LaterException((Piority)((int)piority), message);
    }

    public static void Here()
    {
    }
}