using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRefactor
{

    public static void Later(Piority piority, Potential quality, string additionalMessage = "")
    {
        Later((PiorityExplicit)((int)piority), (PotentialExplicit)((int)quality), additionalMessage);
    }
    public static void Later(PiorityExplicit piority, Potential quality, string additionalMessage = "")
    {
        Later((PiorityExplicit)((int)piority), (PotentialExplicit)((int)quality), additionalMessage);
    }
    public static void Later(Piority piority, PotentialExplicit quality, string additionalMessage = "")
    {
        Later((PiorityExplicit)((int)piority), (PotentialExplicit)((int)quality), additionalMessage);
    }
    public static void Later(PiorityExplicit piority, PotentialExplicit quality, string additionalMessage = "")
    {
        //Use Reflection to connect that to a local DB and GitHub Later
        // For the moment, just CTLR + F "ToRecyle.Later" to find what to recycle on your free time"
    }


    public enum PiorityExplicit : int { AsSoonAsPossible, ComingWeeks, ComingMonth, OneDay }

    public enum PotentialExplicit : int { StorablePackage, ToolboxPackage, RegularUse, Sharable }
}
public enum Piority : int { A, B, C, D }
public enum Potential : int { A, B, C, D }
public enum Quality : int { A, B, C, D }