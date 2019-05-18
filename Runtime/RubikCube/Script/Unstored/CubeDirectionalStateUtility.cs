using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RubikCubeAsDirectionalStateUtility
{
    public static CubeDirectionalState GenerateEmpty()
    {
        return new CubeDirectionalState();
    }
    public static void ResetToEmty(ref CubeDirectionalState cube) {
      CubeFaceState [] faces =  cube.GetAllCubeFaces ();
        for (int i = 0; i < faces.Length; i++)
        {
            faces[i].Reset();
        }
    }

    public static void Rotate(ref CubeDirectionalState cube, RotationTypeShort rotation)
    {
      //  cube.Get(Direction)
    }
}

public interface FaceLinks {

    RubikCubeFace GetFace();
    RubikCubeFaceDirection GetDirection();
    FaceLinks Up();
    FaceLinks Down();
    FaceLinks Left();
    FaceLinks Right();
    FaceLinks Get(ArrowDirection direction, int iteration = 1);
}
public class PieceFaceLinks : FaceLinks
{
    public RubikCubeFace m_face;
    public RubikCubeFaceDirection m_direction;

    public PieceFaceLinks m_up;
    public PieceFaceLinks m_right;
    public PieceFaceLinks m_down;
    public PieceFaceLinks m_left;

    public PieceFaceLinks(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        this.m_face = face;
        this.m_direction = direction;
    }

    public FaceLinks Down()
    {
        return m_down;
    }

    public FaceLinks Get(ArrowDirection direction, int iteration = 1)
    {
        return FaceLinkUtility.Get(this,direction, iteration);
    }

    public RubikCubeFaceDirection GetDirection()
    {
        return m_direction;
    }

    public RubikCubeFace GetFace()
    {
        return m_face;
    }

    public FaceLinks Left()
    {
        return m_left;
    }

    public FaceLinks Right()
    {

        return m_right;
    }

    public FaceLinks Up()
    {
        return m_up;
    }
}
public class FaceLinkUtility
{
    internal static FaceLinks Get(FaceLinks origine, ArrowDirection direction, int iteration=1)
    {
        if (iteration < 0)
        {
            direction = Inverse(direction);
            iteration = Mathf.Abs(iteration);
        }

        FaceLinks result = origine;
        for (int i = 0; i < iteration; i++)
        {
            result = result.Get(direction);
        }
        return result;
    }

    private static ArrowDirection Inverse(ArrowDirection direction)
    {
        if ( direction == ArrowDirection.Up) return ArrowDirection.Down;
        else if ( direction == ArrowDirection.Down) return ArrowDirection.Up;
        else if (  direction == ArrowDirection.Left) return ArrowDirection.Right;
        return ArrowDirection.Left;
    }

    internal static FaceLinks Get(FaceLinks origine, ArrowDirection direction)
    {
        if (direction == ArrowDirection.Up) return origine.Up();
        if (direction == ArrowDirection.Left) return origine.Left();
        if (direction == ArrowDirection.Right) return origine.Right();
        return origine.Down();
    }

    private static bool m_haveBeenDefined;
    private static PieceFaceLinks[] m_allLinks=null;
    public static RubikFaceDictionary<PieceFaceLinks> m_faceDirectAccess = new RubikFaceDictionary<PieceFaceLinks>();

    public static PieceFaceLinks[] GetAllLinks() {
        if (!m_haveBeenDefined) {
            DefineAllLinks();
            m_haveBeenDefined = true;
        }
        return m_allLinks;
    }
    public static PieceFaceLinks GetFaceLinks(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        return m_faceDirectAccess.Get(face, direction);
    }
    public static void DefineAllLinks (){

        List<PieceFaceLinks> faces = new List<PieceFaceLinks>(); ;

        foreach (RubikCubeFace f in Enum.GetValues(typeof(RubikCubeFace)).Cast<RubikCubeFace>().ToList())
        {
            foreach (RubikCubeFaceDirection d in Enum.GetValues(typeof(RubikCubeFaceDirection)).Cast<RubikCubeFaceDirection>().ToList())
            {
                PieceFaceLinks face = new PieceFaceLinks(f, d);
                m_faceDirectAccess.Add(f, d, face);
                faces.Add(face);
            }
        }
        m_allLinks = faces.ToArray();

        //face|direction,
        // selection, up, right, down, left
        string faceLinks = "UP,l,no,u,no,RIGHT,l,no,l,n,DOWN,l,no,l,o,LEFT,l,no,b,so,UP,l,o,l,no,RIGHT,l,o,l,c,DOWN,l,o,l,so,LEFT,l,o,b,o,UP,l,so,l,o,RIGHT,l,so,l,s,DOWN,l,so,d,so,LEFT,l,so,b,no,UP,l,n,u,o,RIGHT,l,n,l,ne,DOWN,l,n,l,c,LEFT,l,n,l,no,UP,l,c,l,n,RIGHT,l,c,l,e,DOWN,l,c,l,s,LEFT,l,c,l,o,UP,l,s,l,c,RIGHT,l,s,l,se,DOWN,l,s,d,o,LEFT,l,s,l,so,UP,l,ne,,,RIGHT,l,ne,f,no,DOWN,l,ne,l,e,LEFT,l,ne,l,n,UP,l,e,l,ne,RIGHT,l,e,f,o,DOWN,l,e,l,se,LEFT,l,e,l,c,UP,l,se,l,e,RIGHT,l,se,f,so,DOWN,l,se,,,LEFT,l,se,l,s,UP,u,no,b,so,RIGHT,u,no,u,n,DOWN,u,no,u,o,LEFT,u,no,l,no,UP,u,o,u,no,RIGHT,u,o,u,c,DOWN,u,o,u,so,LEFT,u,o,l,n,UP,u,so,u,o,RIGHT,u,so,u,s,DOWN,u,so,f,no,LEFT,u,so,,,UP,f,no,u,so,RIGHT,f,no,f,n,DOWN,f,no,f,o,LEFT,f,no,l,ne,UP,f,o,f,no,RIGHT,f,o,f,c,DOWN,f,o,f,so,LEFT,f,o,l,e,UP,f,so,f,o,RIGHT,f,so,f,s,DOWN,f,so,d,no,LEFT,f,so,l,se,UP,d,no,f,so,RIGHT,d,no,d,n,DOWN,d,no,d,o,LEFT,d,no,,,UP,d,o,d,no,RIGHT,d,o,d,c,DOWN,d,o,d,so,LEFT,d,o,l,s,UP,d,so,d,o,RIGHT,d,so,d,s,DOWN,d,so,b,no,LEFT,d,so,l,so,UP,b,no,d,so,RIGHT,b,no,b,n,DOWN,b,no,b,o,LEFT,b,no,l,so,UP,b,o,b,no,RIGHT,b,o,b,c,DOWN,b,o,b,so,LEFT,b,o,l,o,UP,b,so,b,o,RIGHT,b,so,b,s,DOWN,b,so,u,no,LEFT,b,so,l,no,UP,u,n,b,s,RIGHT,u,n,u,ne,DOWN,u,n,u,c,LEFT,u,n,u,no,UP,u,c,u,n,RIGHT,u,c,u,e,DOWN,u,c,u,s,LEFT,u,c,u,o,UP,u,s,u,c,RIGHT,u,s,u,se,DOWN,u,s,f,n,LEFT,u,s,u,so,UP,f,n,u,s,RIGHT,f,n,f,ne,DOWN,f,n,f,c,LEFT,f,n,f,no,UP,f,c,f,n,RIGHT,f,c,f,e,DOWN,f,c,f,s,LEFT,f,c,f,o,UP,f,s,f,c,RIGHT,f,s,f,se,DOWN,f,s,d,n,LEFT,f,s,f,so,UP,d,n,f,s,RIGHT,d,n,d,ne,DOWN,d,n,d,c,LEFT,d,n,d,no,UP,d,c,d,n,RIGHT,d,c,d,e,DOWN,d,c,d,s,LEFT,d,c,d,o,UP,d,s,d,c,RIGHT,d,s,d,se,DOWN,d,s,b,n,LEFT,d,s,d,so,UP,b,n,d,s,RIGHT,b,n,b,ne,DOWN,b,n,b,c,LEFT,b,n,b,no,UP,b,c,b,n,RIGHT,b,c,b,e,DOWN,b,c,b,s,LEFT,b,c,b,o,UP,b,s,b,c,RIGHT,b,s,b,se,DOWN,b,s,u,n,LEFT,b,s,b,so,UP,u,ne,b,se,RIGHT,u,ne,r,ne,DOWN,u,ne,u,e,LEFT,u,ne,u,n,UP,u,e,u,ne,RIGHT,u,e,r,n,DOWN,u,e,u,se,LEFT,u,e,u,c,UP,u,se,u,e,RIGHT,u,se,,,DOWN,u,se,f,ne,LEFT,u,se,u,s,UP,f,ne,u,se,RIGHT,f,ne,r,no,DOWN,f,ne,f,e,LEFT,f,ne,f,n,UP,f,e,f,ne,RIGHT,f,e,r,o,DOWN,f,e,f,se,LEFT,f,e,f,c,UP,f,se,f,e,RIGHT,f,se,r,so,DOWN,f,se,d,ne,LEFT,f,se,f,s,UP,d,ne,f,se,RIGHT,d,ne,,,DOWN,d,ne,d,e,LEFT,d,ne,d,n,UP,d,e,d,ne,RIGHT,d,e,r,s,DOWN,d,e,d,se,LEFT,d,e,d,c,UP,d,se,d,e,RIGHT,d,se,r,se,DOWN,d,se,b,ne,LEFT,d,se,d,s,UP,b,ne,d,se,RIGHT,b,ne,r,se,DOWN,b,ne,b,e,LEFT,b,ne,b,n,UP,b,e,b,ne,RIGHT,b,e,r,e,DOWN,b,e,b,se,LEFT,b,e,b,c,UP,b,se,b,e,RIGHT,b,se,r,ne,DOWN,b,se,u,ne,LEFT,b,se,b,s,UP,r,no,,,RIGHT,r,no,r,n,DOWN,r,no,r,o,LEFT,r,no,f,ne,UP,r,o,r,no,RIGHT,r,o,r,c,DOWN,r,o,r,so,LEFT,r,o,f,e,UP,r,so,r,o,RIGHT,r,so,r,s,DOWN,r,so,,,LEFT,r,so,f,se,UP,r,n,u,e,RIGHT,r,n,r,ne,DOWN,r,n,r,c,LEFT,r,n,r,no,UP,r,c,r,n,RIGHT,r,c,r,e,DOWN,r,c,r,s,LEFT,r,c,r,o,UP,r,s,r,c,RIGHT,r,s,r,se,DOWN,r,s,d,e,LEFT,r,s,r,so,UP,r,ne,u,ne,RIGHT,r,ne,b,se,DOWN,r,ne,r,e,LEFT,r,ne,r,n,UP,r,e,r,ne,RIGHT,r,e,b,e,DOWN,r,e,r,se,LEFT,r,e,r,c,UP,r,se,r,e,RIGHT,r,se,b,ne,DOWN,r,se,d,se,LEFT,r,se,r,s";
        faceLinks = faceLinks.ToLower();
        string[] tokens = faceLinks.Split(',');

        int converted =0;
        for (int j = 0; j < tokens.Length/5; j++)
        {
            bool convertionMistake;
            ArrowDirection dir = GetLinkDirection(tokens[(j * 5) + 0], out convertionMistake);
            if (convertionMistake) continue;
            RubikCubeFace origineFace = GetFace(tokens[j * 5 + 1], out convertionMistake);
            if (convertionMistake) continue;
            RubikCubeFaceDirection originedirection = GetDirection(tokens[j * 5 + 2], out convertionMistake);
            if (convertionMistake) continue;
            RubikCubeFace directionFace = GetFace(tokens[j * 5 + 3], out convertionMistake);
            if (convertionMistake) continue;
            RubikCubeFaceDirection directionDirection = GetDirection(tokens[j * 5 + 4], out convertionMistake);
            if (convertionMistake) continue;

           // Debug.Log("Valide: "+(++ converted));

            PieceFaceLinks face = GetFaceLinks(origineFace, originedirection);
            PieceFaceLinks destination = GetFaceLinks(directionFace, directionDirection);
            switch (dir)
            {
                case ArrowDirection.Left:
                    face.m_left = destination;
                    break;
                case ArrowDirection.Right:
                    face.m_right = destination;
                    break;
                case ArrowDirection.Up:
                    face.m_up = destination;
                    break;
                case ArrowDirection.Down:
                    face.m_down = destination;
                    break;
                default:
                    break;
            }

        }
        Debug.Log("Links Defined: ");

    }

    

    private static RubikCubeFace GetFace(string v, out bool convertionMistake)
    {
        convertionMistake = false;
        switch (v)
        {
            case "f": return RubikCubeFace.Face;
            case "r": return RubikCubeFace.Right;
            case "l": return RubikCubeFace.Left;
            case "u": return RubikCubeFace.Up;
            case "d": return RubikCubeFace.Down;
            case "b": return RubikCubeFace.Back;
            default:
                break;
        }
    convertionMistake=true;
        return RubikCubeFace.Back;
}
    private static RubikCubeFaceDirection GetDirection(string v, out bool convertionMistake)
    {
        convertionMistake = false;
        switch (v)
        {
            case "no": return RubikCubeFaceDirection.NO;
            case "n": return  RubikCubeFaceDirection.N;
            case "ne": return RubikCubeFaceDirection.NE;
            case "o": return  RubikCubeFaceDirection.O;
            case "c": return  RubikCubeFaceDirection.C;
            case "e": return  RubikCubeFaceDirection.E;
            case "so": return RubikCubeFaceDirection.SO;
            case "s": return  RubikCubeFaceDirection.S;
            case "se": return RubikCubeFaceDirection.SE;
            default:
                break;
        }
        convertionMistake = true;
        return RubikCubeFaceDirection.C;
    }

   

    private static ArrowDirection GetLinkDirection(string v, out bool convertionMistake)
    {
        convertionMistake = false;
        switch (v)
        {
            case "u": case "up": return ArrowDirection.Up;
            case "d": case "down": return ArrowDirection.Down;
            case "l": case "left": return ArrowDirection.Left;
            case "r": case "right": return ArrowDirection.Right;
            default:
                break;
        }
        convertionMistake = true;
        return ArrowDirection.Up;
    }
}
