using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRecycle 
{

    public static void Later(Piority piority, Quality quality, string additionalMessage = "") {
        Later((PiorityExplicit)((int)piority), (QualityExplicit)((int)quality), additionalMessage);
    }
    public static void Later(PiorityExplicit piority, Quality quality, string additionalMessage = "")
    {
        Later((PiorityExplicit)((int)piority), (QualityExplicit)((int)quality), additionalMessage);
    }
    public static void Later(Piority piority, QualityExplicit quality, string additionalMessage = "")
    {
        Later((PiorityExplicit)((int)piority), (QualityExplicit)((int)quality), additionalMessage);
    }
    public static void Later(PiorityExplicit piority, QualityExplicit quality, string additionalMessage = "")
    {
        //Use Reflection to connect that to a local DB and GitHub Later
        // For the moment, just CTLR + F "ToRecyle.Later" to find what to recycle on your free time"
    }
    
    public enum PiorityExplicit : int { AsSoonAsPossible, ComingWeeks, ComingMonth, OneDay }
    public enum QualityExplicit : int { StorablePackage, ToolboxPackage, RegularUse, CouldBeNiceToShare}
}
