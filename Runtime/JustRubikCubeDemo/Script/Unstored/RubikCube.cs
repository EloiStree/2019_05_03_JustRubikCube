using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IRubikCube
{

    TagRubikCube GetLinkedCube();
    TagRubikCubeFace GetLinkedFace(RubikCubeFace face, RubikCubeFaceDirection direction);
    TagRubikCubePiece GetLinkedPiece(RubikPieceByPosition3D position);
    TagRubikCubePivot GetLinkedPivot(RubikCubePivotable pivot);

}

public class RubikCube
{
    static RubikCube()
    {
        FlyWeighInitialisation();
    }

    public static Color m_white = Color.white;
    public static Color m_green = Color.green;
    public static Color m_yellow = Color.yellow;
    public static Color m_orange = new Color(255f / 255f, 165f / 255f, 0f);
    public static Color m_blue = Color.blue;
    public static Color m_red = Color.red;

    public static Color GetDefaultColorOfCube(RubikCubeColor color)
    {
        switch (color)
        {
            case RubikCubeColor.White:
                return m_white;
            case RubikCubeColor.Red:
                return m_red;
            case RubikCubeColor.Green:
                return m_green;
            case RubikCubeColor.Blue:
                return m_blue;
            case RubikCubeColor.Orange:
                return m_orange;
            case RubikCubeColor.Yellow:
                return m_yellow;
            default:
                break;
        }
        return Color.black;
    }


    public static RubikCubeColor GetDefaultColor(RubikCubeFace face)
    {
        switch (face)
        {
            case RubikCubeFace.Left:
                return RubikCubeColor.White;
            case RubikCubeFace.Right:
                return RubikCubeColor.Yellow;
            case RubikCubeFace.Up:
                return RubikCubeColor.Red;
            case RubikCubeFace.Down:
                return RubikCubeColor.Orange;
            case RubikCubeFace.Face:
                return RubikCubeColor.Green;
            case RubikCubeFace.Back:
                return RubikCubeColor.Blue;
            default:
                break;
        }
        return RubikCubeColor.White;
    }



    public static RotationTypeShort[] InverseOf(RotationTypeShort[] rotations)
    {
        RotationTypeShort[] result = new RotationTypeShort[rotations.Length];
        for (int i = 0; i < rotations.Length; i++)
        {
            result[rotations.Length - 1 - i] = RubikCube.GetInverseOf(rotations[i]);
        }
        return result;
    }
    public static RotationTypeShort ConvertAsShort(RotationTypeLong value)
    {
        switch (value)
        {
            case RotationTypeLong.Left: return RotationTypeShort.L;
            case RotationTypeLong.LeftCounter: return RotationTypeShort.Lp;
            case RotationTypeLong.Right: return RotationTypeShort.R;
            case RotationTypeLong.RightCounter: return RotationTypeShort.Rp;
            case RotationTypeLong.Up: return RotationTypeShort.U;
            case RotationTypeLong.UpCounter: return RotationTypeShort.Rp;
            case RotationTypeLong.Down: return RotationTypeShort.D;
            case RotationTypeLong.DownCounter: return RotationTypeShort.Dp;
            case RotationTypeLong.Face: return RotationTypeShort.F;
            case RotationTypeLong.FaceCounter: return RotationTypeShort.Fp;
            case RotationTypeLong.Back: return RotationTypeShort.B;
            case RotationTypeLong.BackCounter: return RotationTypeShort.Bp;
            case RotationTypeLong.Middle: return RotationTypeShort.M;
            case RotationTypeLong.MiddleCounter: return RotationTypeShort.Mp;
            case RotationTypeLong.Equator: return RotationTypeShort.E;
            case RotationTypeLong.EquatorCounter: return RotationTypeShort.Ep;
            case RotationTypeLong.Standing: return RotationTypeShort.S;
            case RotationTypeLong.StandingCounter: return RotationTypeShort.Sp;
            default:
                return RotationTypeShort.L;
        }
    }
    public static RotationTypeShort GetInverseOf(RotationTypeShort value)
    {
        switch (value)
        {
            case RotationTypeShort.L:
                return RotationTypeShort.Lp;
            case RotationTypeShort.Lp:
                return RotationTypeShort.L;
            case RotationTypeShort.R:
                return RotationTypeShort.Rp;
            case RotationTypeShort.Rp:
                return RotationTypeShort.R;
            case RotationTypeShort.U:
                return RotationTypeShort.Up;
            case RotationTypeShort.Up:
                return RotationTypeShort.U;
            case RotationTypeShort.D:
                return RotationTypeShort.Dp;
            case RotationTypeShort.Dp:
                return RotationTypeShort.D;
            case RotationTypeShort.F:
                return RotationTypeShort.Fp;
            case RotationTypeShort.Fp:
                return RotationTypeShort.F;
            case RotationTypeShort.B:
                return RotationTypeShort.Bp;
            case RotationTypeShort.Bp:
                return RotationTypeShort.B;
            case RotationTypeShort.M:
                return RotationTypeShort.Mp;
            case RotationTypeShort.Mp:
                return RotationTypeShort.M;
            case RotationTypeShort.E:
                return RotationTypeShort.Ep;
            case RotationTypeShort.Ep:
                return RotationTypeShort.E;
            case RotationTypeShort.S:
                return RotationTypeShort.Sp;
            case RotationTypeShort.Sp:
                return RotationTypeShort.S;
            default:
                return RotationTypeShort.Lp;
        }
    }

    public static RotationTypeLong ConvertAcronymShortToLong(RotationTypeShort rotationTypeShort)
    {
        switch (rotationTypeShort)
        {
            case RotationTypeShort.L: return RotationTypeLong.Left;
            case RotationTypeShort.Lp: return RotationTypeLong.LeftCounter;
            case RotationTypeShort.R: return RotationTypeLong.Right;
            case RotationTypeShort.Rp: return RotationTypeLong.RightCounter;
            case RotationTypeShort.U: return RotationTypeLong.Up;
            case RotationTypeShort.Up: return RotationTypeLong.UpCounter;
            case RotationTypeShort.D: return RotationTypeLong.Down;
            case RotationTypeShort.Dp: return RotationTypeLong.DownCounter;
            case RotationTypeShort.F: return RotationTypeLong.Face;
            case RotationTypeShort.Fp: return RotationTypeLong.FaceCounter;
            case RotationTypeShort.B: return RotationTypeLong.Back;
            case RotationTypeShort.Bp: return RotationTypeLong.BackCounter;
            case RotationTypeShort.M: return RotationTypeLong.Middle;
            case RotationTypeShort.Mp: return RotationTypeLong.MiddleCounter;
            case RotationTypeShort.E: return RotationTypeLong.Equator;
            case RotationTypeShort.Ep: return RotationTypeLong.EquatorCounter;
            case RotationTypeShort.S: return RotationTypeLong.Standing;
            case RotationTypeShort.Sp: return RotationTypeLong.StandingCounter;

            default:
                return RotationTypeLong.Back;

        }
    }
    public static void ConvertAcronymToFaceRotation(RotationTypeShort instruction, out RubikCubePivotable faceToRotate, out bool clockwise)
    {
        switch (instruction)
        {
            case RotationTypeShort.L:
                clockwise = true; faceToRotate = RubikCubePivotable.Left;
                break;
            case RotationTypeShort.Lp:
                clockwise = false; faceToRotate = RubikCubePivotable.Left;
                break;
            case RotationTypeShort.R:
                clockwise = true; faceToRotate = RubikCubePivotable.Right;
                break;
            case RotationTypeShort.Rp:
                clockwise = false; faceToRotate = RubikCubePivotable.Right;
                break;
            case RotationTypeShort.U:
                clockwise = true; faceToRotate = RubikCubePivotable.Up;
                break;
            case RotationTypeShort.Up:
                clockwise = false; faceToRotate = RubikCubePivotable.Up;
                break;
            case RotationTypeShort.D:
                clockwise = true; faceToRotate = RubikCubePivotable.Down;
                break;
            case RotationTypeShort.Dp:
                clockwise = false; faceToRotate = RubikCubePivotable.Down;
                break;
            case RotationTypeShort.F:
                clockwise = true; faceToRotate = RubikCubePivotable.Face;
                break;
            case RotationTypeShort.Fp:
                clockwise = false; faceToRotate = RubikCubePivotable.Face;
                break;
            case RotationTypeShort.B:
                clockwise = true; faceToRotate = RubikCubePivotable.Back;
                break;
            case RotationTypeShort.Bp:
                clockwise = false; faceToRotate = RubikCubePivotable.Back;
                break;
            case RotationTypeShort.M:
                clockwise = true; faceToRotate = RubikCubePivotable.Middle;
                break;
            case RotationTypeShort.Mp:
                clockwise = false; faceToRotate = RubikCubePivotable.Middle;
                break;
            case RotationTypeShort.E:
                clockwise = true; faceToRotate = RubikCubePivotable.Equator;
                break;
            case RotationTypeShort.Ep:
                clockwise = false; faceToRotate = RubikCubePivotable.Equator;
                break;
            case RotationTypeShort.S:
                clockwise = true; faceToRotate = RubikCubePivotable.Standing;
                break;
            case RotationTypeShort.Sp:
                clockwise = false; faceToRotate = RubikCubePivotable.Standing;
                break;
            default:
                clockwise = true; faceToRotate = RubikCubePivotable.Middle;
                break;
        }
    }
    public static string ConvertAcronymShortToString(RotationTypeShort rotation)
    {
        RubikCubePivotable face;
        bool clockwise;
        ConvertAcronymToFaceRotation(rotation, out face, out clockwise);
        return ConvertFaceRotationToString(face, clockwise);

    }
    public static string ConvertFaceRotationToString(RubikCubePivotable faceToRotate, bool clockWise)
    {
        RotationTypeShort rotType = ConvertRotationToAcronymShort(faceToRotate, clockWise);
        switch (rotType)
        {
            case RotationTypeShort.L: return "L";
            case RotationTypeShort.Lp: return "L'";
            case RotationTypeShort.R: return "R";
            case RotationTypeShort.Rp: return "R'";
            case RotationTypeShort.U: return "U";
            case RotationTypeShort.Up: return "U'";
            case RotationTypeShort.D: return "D";
            case RotationTypeShort.Dp: return "D'";
            case RotationTypeShort.F: return "F";
            case RotationTypeShort.Fp: return "F'";
            case RotationTypeShort.B: return "B";
            case RotationTypeShort.Bp: return "B'";
            case RotationTypeShort.M: return "M";
            case RotationTypeShort.Mp: return "M'";
            case RotationTypeShort.E: return "E";
            case RotationTypeShort.Ep: return "E'";
            case RotationTypeShort.S: return "S";
            case RotationTypeShort.Sp: return "S'";
            default: return "?";
        }


    }
    public static RotationTypeShort ConvertRotationToAcronymShort(RubikCubePivotable faceToRotate, bool clockWise)
    {
        switch (faceToRotate)
        {
            case RubikCubePivotable.Left: return clockWise ? RotationTypeShort.L : RotationTypeShort.Lp;
            case RubikCubePivotable.Right: return clockWise ? RotationTypeShort.R : RotationTypeShort.Rp;
            case RubikCubePivotable.Up: return clockWise ? RotationTypeShort.U : RotationTypeShort.Up;
            case RubikCubePivotable.Down: return clockWise ? RotationTypeShort.D : RotationTypeShort.Dp;
            case RubikCubePivotable.Face: return clockWise ? RotationTypeShort.F : RotationTypeShort.Fp;
            case RubikCubePivotable.Back: return clockWise ? RotationTypeShort.B : RotationTypeShort.Bp;
            case RubikCubePivotable.Middle: return clockWise ? RotationTypeShort.M : RotationTypeShort.Mp;
            case RubikCubePivotable.Equator: return clockWise ? RotationTypeShort.E : RotationTypeShort.Ep;
            case RubikCubePivotable.Standing: return clockWise ? RotationTypeShort.S : RotationTypeShort.Sp;
            default:
                return clockWise ? RotationTypeShort.E : RotationTypeShort.Ep;
        }
    }

    public static bool ConvertStringToShortAcronym(string requestAcryonym, out RotationTypeShort type)
    {
        requestAcryonym = requestAcryonym.ToLower();
        switch (requestAcryonym)
        {
            case "b": type = RotationTypeShort.B; break;
            case "b'": case "bp": case "bi": type = RotationTypeShort.Bp; break;
            case "d": type = RotationTypeShort.D; break;
            case "d'": case "dp": case "di": type = RotationTypeShort.Dp; break;
            case "e": type = RotationTypeShort.E; break;
            case "e'": case "ep": case "ei": type = RotationTypeShort.Ep; break;
            case "f": type = RotationTypeShort.F; break;
            case "f'": case "fp": case "fi": type = RotationTypeShort.Fp; break;
            case "l": type = RotationTypeShort.L; break;
            case "l'": case "lp": case "li": type = RotationTypeShort.Lp; break;
            case "m": type = RotationTypeShort.M; break;
            case "m'": case "mp": case "mi": type = RotationTypeShort.Mp; break;

            case "r": type = RotationTypeShort.R; break;
            case "r'": case "rp": case "ri": type = RotationTypeShort.Rp; break;

            case "s": type = RotationTypeShort.S; break;
            case "s'": case "sp": case "si": type = RotationTypeShort.Sp; break;

            case "u": type = RotationTypeShort.U; break;
            case "u'": case "up": case "ui": type = RotationTypeShort.Up; break;

            default:
                type = RotationTypeShort.B;
                return false;
        }
        return true;
    }

    public static List<T> GetListOf<T>() { return Enum.GetValues(typeof(T)).Cast<T>().ToList(); }
    public static T[] GetArrayOf<T>() { return Enum.GetValues(typeof(T)).Cast<T>().ToArray(); }
    public static T GetEnumRandom<T>()
    {
        List<T> l = GetListOf<T>();
        return l[UnityEngine.Random.Range(0, l.Count)];
    }
    public static RotationTypeShort GetRandomShort()
    {
        return GetEnumRandom<RotationTypeShort>();
    }
    public static RotationTypeLong GetRandomLong()
    {
        return GetEnumRandom<RotationTypeLong>();
    }

    public static RubikCubePivotable GetRandomFace()
    {
        int face = UnityEngine.Random.Range(0, 9);
        switch (face)
        {
            case 0: return RubikCubePivotable.Back;
            case 1: return RubikCubePivotable.Down;
            case 2: return RubikCubePivotable.Equator;
            case 3: return RubikCubePivotable.Face;
            case 4: return RubikCubePivotable.Left;
            case 5: return RubikCubePivotable.Middle;
            case 6: return RubikCubePivotable.Right;
            case 7: return RubikCubePivotable.Standing;
            case 8: return RubikCubePivotable.Up;
            default:
                return RubikCubePivotable.Middle;
        }
    }

    public static string GetTagOf(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        return face.ToString() + "|" + direction.ToString();

    }
    public static string GetTagOf(RubikPieceByPosition3D position)
    {
        return position.ToString();
    }
    public static string GetTagOf(RubikPiecePositionByPivot position)
    {
        return position.ToString();
    }
    public static string GetTagOf(RubikCubePivotable[] pivot)
    {
        string s = "";
        for (int i = 0; i < pivot.Length; i++)
        {
            s += (i == 0 ? "" : "|") + pivot[i].ToString();
        }
        return s;
    }


    public static RubikPieceByPosition3D GetPiecePositionBasedOn(RubikCubeDepth depth, RubikCubeFaceDirection direction)
    {

        int x = 1;
        int y = 1;
        int z = (int)depth;
        switch (direction)
        {
            case RubikCubeFaceDirection.SO: x = 0; y = 0; break;
            case RubikCubeFaceDirection.S: x = 1; y = 0; break;
            case RubikCubeFaceDirection.SE: x = 2; y = 0; break;
            case RubikCubeFaceDirection.O: x = 0; y = 1; break;
            case RubikCubeFaceDirection.C: x = 1; y = 1; break;
            case RubikCubeFaceDirection.E: x = 2; y = 1; break;
            case RubikCubeFaceDirection.NO: x = 0; y = 2; break;
            case RubikCubeFaceDirection.N: x = 1; y = 2; break;
            case RubikCubeFaceDirection.NE: x = 2; y = 2; break;
            default:
                break;
        }
        return GetPieceOf(x, y, z).GetPosition3D();
    }
    #region NOT IMPLEMENTED YET
    public static RubikPiecePositionByPivot GetPiecePositionBasedOn(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        throw new System.NotImplementedException();
    }

    public static RubikPiecePositionByPivot GetPiecePositionBasedOn(RubikCubePivotable[] pivots)
    {
        throw new NotImplementedException();
    }

    public static RubikCubePivotable[] GetPivotsFrom(RubikPiecePositionByPivot position)
    {
        throw new NotImplementedException();

    }
    #endregion


    #region QUICK ACCESS TO STATIC DATA ON THE RUBIK CUBE
    public static RubikCubePieceIndex GetPieceOf(int x, int y, int z)
    {
        return RubikCubePieceIndex.GetIndex(x, y, z);
    }
    public static RubikCubePieceIndex GetPieceOf(RubikPieceByPosition3D position3D)
    {
        return RubikCubePieceIndex.GetIndex(position3D);
    }

    public static RubikCubePieceIndex GetPieceOf(RubikPiecePositionByPivot pivot)
    {

        return RubikCubePieceIndex.GetPieceIndex(pivot);
    }


    public static RubikCubePieceIndex GetPieceOf(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        throw new NotImplementedException();
    }
    private static void GetIntPosition(RubikPieceByPosition3D position, out short x, out short y, out short z)
    {
        throw new NotImplementedException();
    }
    #endregion

    public class RubikCubePieceIndex
    {


        private static RubikCubePieceIndex[] m_indexTable = new RubikCubePieceIndex[27];
        private static RubikCubePieceIndex[,,] m_indexmatrice = new RubikCubePieceIndex[3, 3, 3];
        private static RubikPieceByPosition3D[,,] m_positionMatrice = new RubikPieceByPosition3D[3, 3, 3];
        public int m_index;
        public RubikPieceByPosition3D m_positionByVector3;
        public int m_x;
        public int m_y;
        public int m_z;
        public RubikPiecePositionByPivot m_positionByPivots;
        private RubikCubePivotable m_vertical;
        private RubikCubePivotable m_horizontal;
        private RubikCubePivotable m_standing;

        public RubikCubePieceIndex(RubikPieceByPosition3D position3D, int x, int y, int z, RubikPiecePositionByPivot positionPivot, RubikCubePivotable vertical, RubikCubePivotable horizontal, RubikCubePivotable standing)
        {
            this.m_index = (int)position3D;
            this.m_positionByVector3 = position3D;
            this.m_x = x;
            this.m_y = y;
            this.m_z = z;
            this.m_positionByPivots = positionPivot;
            this.m_vertical = vertical;
            this.m_horizontal = horizontal;
            this.m_standing = standing;
            m_indexTable[m_index] = this;
            m_indexmatrice[x, y, z] = this;
            m_positionMatrice[x, y, z] = position3D;
        }

        public static RubikCubePieceIndex GetIndex(int x, int y, int z)
        {
            if (x <= 0) x = 0;
            if (y <= 0) y = 0;
            if (z <= 0) z = 0;
            if (x > 2) x = 2;
            if (y > 2) y = 2;
            if (z > 2) z = 2;
            return m_indexmatrice[x, y, z];

        }
        public static RubikPieceByPosition3D GetPosition3D(int x, int y, int z)
        {
            if (x <= 0) x = 0;
            if (y <= 0) y = 0;
            if (z <= 0) z = 0;
            if (x > 2) x = 2;
            if (y > 2) y = 2;
            if (z > 2) z = 2;
            return m_positionMatrice[x, y, z];

        }

        public RubikPieceByPosition3D GetPosition3D()
        {
            return m_positionByVector3;
        }

        public RubikCubePivotable[] GetPivots()
        {
            return new RubikCubePivotable[] { m_horizontal, m_vertical, m_standing };
        }

        public static RubikCubePieceIndex GetIndex(RubikPieceByPosition3D position3D)
        {
            return m_indexTable[(int)position3D];
        }

        public static RubikCubePieceIndex GetPieceIndex(RubikPiecePositionByPivot pivot)
        {
            for (int i = 0; i < m_indexTable.Length; i++)
            {
                if (m_indexTable[i].m_positionByPivots == pivot)
                    return m_indexTable[i];
            }
            return null;
        }
    }
    [System.Serializable]

    public class RubikCubeFaceIndex
    {
        public RubikCubeFace m_face;
        public RubikCubeFaceDirection m_direction;
        public RubikPieceByPosition3D m_positionReference;
        public CrossVector m_crossPosition = new CrossVector();

        public RubikCubeFaceIndex(short crossX, short crossY, RubikPieceByPosition3D positionReference, RubikCubeFace face, RubikCubeFaceDirection direction)
        {
            this.m_positionReference = positionReference;
            this.m_face = face;
            this.m_direction = direction;
            this.m_crossPosition.x = crossX;
            this.m_crossPosition.y = crossY;
        }
        public class CrossVector
        {
            public short x, y;
        }
    }
    #region INDEX

    private static RubikCubeFaceIndex[] facesIndex = null;
    private static RubikCubePieceIndex[] piecesIndex = null;
    public static void FlyWeighInitialisation()
    {
        if (facesIndex == null)
            facesIndex = GetFaceIndex();
        if (piecesIndex == null)
            piecesIndex = GetPieceIndex();

    }

    private static RubikCubePieceIndex[] GetPieceIndex()
    {
        return new RubikCubePieceIndex[] {
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y0_Z0, 0,0,0,RubikPiecePositionByPivot.L_D_F ,RubikCubePivotable.Left  , RubikCubePivotable.Down , RubikCubePivotable.Face     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y0_Z0, 1,0,0,RubikPiecePositionByPivot.M_D_F ,RubikCubePivotable.Middle, RubikCubePivotable.Down , RubikCubePivotable.Face     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y0_Z0, 2,0,0,RubikPiecePositionByPivot.R_D_F ,RubikCubePivotable.Right , RubikCubePivotable.Down , RubikCubePivotable.Face     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y0_Z1, 0,0,1,RubikPiecePositionByPivot.L_D_S ,RubikCubePivotable.Left  , RubikCubePivotable.Down , RubikCubePivotable.Standing ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y0_Z1, 1,0,1,RubikPiecePositionByPivot.M_D_S ,RubikCubePivotable.Middle, RubikCubePivotable.Down , RubikCubePivotable.Standing ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y0_Z1, 2,0,1,RubikPiecePositionByPivot.R_D_S ,RubikCubePivotable.Right , RubikCubePivotable.Down , RubikCubePivotable.Standing ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y0_Z2, 0,0,2,RubikPiecePositionByPivot.L_D_B ,RubikCubePivotable.Left  , RubikCubePivotable.Down , RubikCubePivotable.Back     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y0_Z2, 1,0,2,RubikPiecePositionByPivot.M_D_B ,RubikCubePivotable.Middle, RubikCubePivotable.Down , RubikCubePivotable.Back     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y0_Z2, 2,0,2,RubikPiecePositionByPivot.R_D_B ,RubikCubePivotable.Right , RubikCubePivotable.Down , RubikCubePivotable.Back     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y1_Z0, 0,1,0,RubikPiecePositionByPivot.L_E_F ,RubikCubePivotable.Left  , RubikCubePivotable.Equator , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y1_Z0, 1,1,0,RubikPiecePositionByPivot.M_E_F ,RubikCubePivotable.Middle, RubikCubePivotable.Equator , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y1_Z0, 2,1,0,RubikPiecePositionByPivot.R_E_F ,RubikCubePivotable.Right , RubikCubePivotable.Equator , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y1_Z1, 0,1,1,RubikPiecePositionByPivot.L_E_S ,RubikCubePivotable.Left  , RubikCubePivotable.Equator , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y1_Z1, 1,1,1,RubikPiecePositionByPivot.M_E_S ,RubikCubePivotable.Middle, RubikCubePivotable.Equator , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y1_Z1, 2,1,1,RubikPiecePositionByPivot.R_E_S ,RubikCubePivotable.Right , RubikCubePivotable.Equator , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y1_Z2, 0,1,2,RubikPiecePositionByPivot.L_E_B ,RubikCubePivotable.Left  , RubikCubePivotable.Equator , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y1_Z2, 1,1,2,RubikPiecePositionByPivot.M_E_B ,RubikCubePivotable.Middle, RubikCubePivotable.Equator , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y1_Z2, 2,1,2,RubikPiecePositionByPivot.R_E_B ,RubikCubePivotable.Right , RubikCubePivotable.Equator , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y2_Z0, 0,2,0,RubikPiecePositionByPivot.L_U_F ,RubikCubePivotable.Left  , RubikCubePivotable.Up , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y2_Z0, 1,2,0,RubikPiecePositionByPivot.M_U_F ,RubikCubePivotable.Middle, RubikCubePivotable.Up , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y2_Z0, 2,2,0,RubikPiecePositionByPivot.R_U_F ,RubikCubePivotable.Right , RubikCubePivotable.Up , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y2_Z1, 0,2,1,RubikPiecePositionByPivot.L_U_S ,RubikCubePivotable.Left  , RubikCubePivotable.Up , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y2_Z1, 1,2,1,RubikPiecePositionByPivot.M_U_S ,RubikCubePivotable.Middle, RubikCubePivotable.Up , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y2_Z1, 2,2,1,RubikPiecePositionByPivot.R_U_S ,RubikCubePivotable.Right , RubikCubePivotable.Up , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y2_Z2, 0,2,2,RubikPiecePositionByPivot.L_U_B ,RubikCubePivotable.Left  , RubikCubePivotable.Up , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y2_Z2, 1,2,2,RubikPiecePositionByPivot.M_U_B ,RubikCubePivotable.Middle, RubikCubePivotable.Up , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y2_Z2, 2,2,2,RubikPiecePositionByPivot.R_U_B ,RubikCubePivotable.Right , RubikCubePivotable.Up , RubikCubePivotable.Back      )

    };
    }
    private static RubikCubeFaceIndex[] GetFaceIndex()
    {
        return new RubikCubeFaceIndex[] {
        new RubikCubeFaceIndex(0,8,RubikPieceByPosition3D.X0_Y2_Z2, RubikCubeFace.Left, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(1,8,RubikPieceByPosition3D.X0_Y2_Z1, RubikCubeFace.Left, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(2,8,RubikPieceByPosition3D.X0_Y2_Z0, RubikCubeFace.Left, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(0,7,RubikPieceByPosition3D.X0_Y1_Z2, RubikCubeFace.Left, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(1,7,RubikPieceByPosition3D.X0_Y1_Z1, RubikCubeFace.Left, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(2,7,RubikPieceByPosition3D.X0_Y1_Z0, RubikCubeFace.Left, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(0,6,RubikPieceByPosition3D.X0_Y0_Z2, RubikCubeFace.Left, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(1,6,RubikPieceByPosition3D.X0_Y0_Z1, RubikCubeFace.Left, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(2,6,RubikPieceByPosition3D.X0_Y0_Z0, RubikCubeFace.Left, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(3,8,RubikPieceByPosition3D.X0_Y2_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,8,RubikPieceByPosition3D.X1_Y2_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(5,8,RubikPieceByPosition3D.X2_Y2_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(3,7,RubikPieceByPosition3D.X0_Y1_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,7,RubikPieceByPosition3D.X1_Y1_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(5,7,RubikPieceByPosition3D.X2_Y1_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(3,6,RubikPieceByPosition3D.X0_Y0_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,6,RubikPieceByPosition3D.X1_Y0_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(5,6,RubikPieceByPosition3D.X2_Y0_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(6,8,RubikPieceByPosition3D.X2_Y2_Z0, RubikCubeFace.Right, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(7,8,RubikPieceByPosition3D.X2_Y2_Z1, RubikCubeFace.Right, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(8,8,RubikPieceByPosition3D.X2_Y2_Z2, RubikCubeFace.Right, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(6,7,RubikPieceByPosition3D.X2_Y1_Z0, RubikCubeFace.Right, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(7,7,RubikPieceByPosition3D.X2_Y1_Z1, RubikCubeFace.Right, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(8,7,RubikPieceByPosition3D.X2_Y1_Z2, RubikCubeFace.Right, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(6,6,RubikPieceByPosition3D.X2_Y0_Z0, RubikCubeFace.Right, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(7,6,RubikPieceByPosition3D.X2_Y0_Z1, RubikCubeFace.Right, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(8,6,RubikPieceByPosition3D.X2_Y0_Z2, RubikCubeFace.Right, RubikCubeFaceDirection.SE),


        new RubikCubeFaceIndex(3,11,RubikPieceByPosition3D.X0_Y2_Z2, RubikCubeFace.Up, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,11,RubikPieceByPosition3D.X1_Y2_Z2, RubikCubeFace.Up, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(5,11,RubikPieceByPosition3D.X2_Y2_Z2, RubikCubeFace.Up, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(3,10,RubikPieceByPosition3D.X0_Y2_Z1, RubikCubeFace.Up, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,10,RubikPieceByPosition3D.X1_Y2_Z1, RubikCubeFace.Up, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(5,10,RubikPieceByPosition3D.X2_Y2_Z1, RubikCubeFace.Up, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(3,9 ,RubikPieceByPosition3D.X0_Y2_Z0, RubikCubeFace.Up, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,9 ,RubikPieceByPosition3D.X1_Y2_Z0, RubikCubeFace.Up, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(5,9 ,RubikPieceByPosition3D.X2_Y2_Z0, RubikCubeFace.Up, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(3,5,RubikPieceByPosition3D.X0_Y0_Z0, RubikCubeFace.Down, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,5,RubikPieceByPosition3D.X1_Y0_Z0, RubikCubeFace.Down, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(5,5,RubikPieceByPosition3D.X2_Y0_Z0, RubikCubeFace.Down, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(3,4,RubikPieceByPosition3D.X0_Y0_Z1, RubikCubeFace.Down, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,4,RubikPieceByPosition3D.X1_Y0_Z1, RubikCubeFace.Down, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(5,4,RubikPieceByPosition3D.X2_Y0_Z1, RubikCubeFace.Down, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(3,3,RubikPieceByPosition3D.X0_Y0_Z2, RubikCubeFace.Down, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,3,RubikPieceByPosition3D.X1_Y0_Z2, RubikCubeFace.Down, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(5,3,RubikPieceByPosition3D.X2_Y0_Z2, RubikCubeFace.Down, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(5,0,RubikPieceByPosition3D.X2_Y2_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,0,RubikPieceByPosition3D.X1_Y2_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(3,0,RubikPieceByPosition3D.X0_Y2_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(5,1,RubikPieceByPosition3D.X2_Y1_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,1,RubikPieceByPosition3D.X1_Y1_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(3,1,RubikPieceByPosition3D.X0_Y1_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(5,2,RubikPieceByPosition3D.X2_Y0_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,2,RubikPieceByPosition3D.X1_Y0_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(3,2,RubikPieceByPosition3D.X0_Y0_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.SE)

    };
    }
    #endregion



    public static RotationTypeShort[] ConvertStringToShorts(string sequence)
    {
        List<RotationTypeShort> rotations = new List<RotationTypeShort>();

        string[] seg = sequence.Split(new char[] { ' ', ':', ';', ',' });
        foreach (string str in seg)
        {

            RotationTypeShort rotationType;
            if (RubikCube.ConvertStringToShortAcronym(str, out rotationType))
            {
                rotations.Add(rotationType);
            }

        }
        return rotations.ToArray();

    }


    public static RotationSequence GetRandomSequence(int randomIteration)
    {
        return new RotationSequence(GetRandomShortSequence(randomIteration));
    }

    public static IEnumerable<RotationTypeShort> GetRandomShortSequence(int randomIteration)
    {
        List<RotationTypeShort> rotations = new List<RotationTypeShort>();
        for (int i = 0; i < randomIteration; i++)
            rotations.Add(RubikCube.GetRandomShort());
        return rotations;
    }

    public static bool GetFaceInfoInString(string text, out RubikCubeFace face)
    {
        face = RubikCubeFace.Back;
        text = text.ToLower();
        foreach (RubikCubeFace f in RubikCube.GetListOf<RubikCubeFace>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                face = f;
                return true;
            }
        }
        return false;
    }

    public static bool GetDirectionInfoInString(string text, out RubikCubeFaceDirection direction)
    {
        direction = RubikCubeFaceDirection.C;
        text = text.ToLower();
        foreach (RubikCubeFaceDirection f in RubikCube.GetListOf<RubikCubeFaceDirection>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                direction = f;
                return true;
            }
        }
        return false;
    }
    public static bool GetPositionInfoInString(string text, out RubikPieceByPosition3D position)
    {
        position = RubikPieceByPosition3D.X0_Y0_Z0;
        text = text.ToLower();
        foreach (RubikPieceByPosition3D f in RubikCube.GetListOf<RubikPieceByPosition3D>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                position = f;
                return true;
            }
        }
        return false;
    }
    public static bool GetPositionInfoInString(string text, out RubikPiecePositionByPivot position)
    {
        position = RubikPiecePositionByPivot.M_E_S;
        text = text.ToLower();
        foreach (RubikPiecePositionByPivot f in RubikCube.GetListOf<RubikPiecePositionByPivot>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                position = f;
                return true;
            }
        }
        return false;

    }


    #region CUBE DIRECTIONAL STATE
    public static CubeDirectionalState CreateCubeStateFrom(RotationSequence rotationSequence)
    {

        if (RubikCubeEngineMono.m_fakeCubeInBackground)
        {
            RubikCubeEngineMono.m_fakeCubeInBackground.ResetInitialState();
            RubikCubeEngineMono.m_fakeCubeInBackground.AddLocalRotationSequence(rotationSequence);
            RubikCubeEngineMono.m_fakeCubeInBackground.FinishMotorQueuedRotation();
            return RubikCubeEngineMono.m_fakeCubeInBackground.GetCubeState();

        }
        return null;
    }
    #endregion

    public static RotationTypeShort GetInvertOf(RotationTypeShort rotationTypeShort)
    {
        switch (rotationTypeShort)
        {
            case RotationTypeShort.L: return RotationTypeShort.Lp;
            case RotationTypeShort.Lp: return RotationTypeShort.L;
            case RotationTypeShort.R: return RotationTypeShort.Rp;
            case RotationTypeShort.Rp: return RotationTypeShort.R;
            case RotationTypeShort.U: return RotationTypeShort.Up;
            case RotationTypeShort.Up: return RotationTypeShort.U;
            case RotationTypeShort.D: return RotationTypeShort.Dp;
            case RotationTypeShort.Dp: return RotationTypeShort.D;
            case RotationTypeShort.F: return RotationTypeShort.Fp;
            case RotationTypeShort.Fp: return RotationTypeShort.F;
            case RotationTypeShort.B: return RotationTypeShort.Bp;
            case RotationTypeShort.Bp: return RotationTypeShort.B;
            case RotationTypeShort.M: return RotationTypeShort.Mp;
            case RotationTypeShort.Mp: return RotationTypeShort.M;
            case RotationTypeShort.E: return RotationTypeShort.Ep;
            case RotationTypeShort.Ep: return RotationTypeShort.E;
            case RotationTypeShort.S: return RotationTypeShort.Sp;
            case RotationTypeShort.Sp: return RotationTypeShort.S;
            default:
                throw new Exception("Shout not be possible");
        }
    }

    public static RubikCubeColor GetColor(RubikCubePivotable pivot, out bool iSNotDefined)
    {
        iSNotDefined = false;
        switch (pivot)
        {
            case RubikCubePivotable.Left: return RubikCubeColor.White;
            case RubikCubePivotable.Right: return RubikCubeColor.Yellow;
            case RubikCubePivotable.Up: return RubikCubeColor.Red;
            case RubikCubePivotable.Down: return RubikCubeColor.Orange;
            case RubikCubePivotable.Face: return RubikCubeColor.Green;
            case RubikCubePivotable.Back: return RubikCubeColor.Blue;
        }
        iSNotDefined = true;
        return RubikCubeColor.White;
    }
    public static RubikCubeColor GetColor(RubikCubeFace face)
    {
        switch (face)
        {
            case RubikCubeFace.Left: return RubikCubeColor.White;
            case RubikCubeFace.Right: return RubikCubeColor.Yellow;
            case RubikCubeFace.Up: return RubikCubeColor.Red;
            case RubikCubeFace.Down: return RubikCubeColor.Orange;
            case RubikCubeFace.Face: return RubikCubeColor.Green;
            case RubikCubeFace.Back: return RubikCubeColor.Blue;
            default:
                throw new Exception("No color define");
        }
    }

    public static Color GetColor(RubikCubeColor colorType)
    {
        switch (colorType)
        {
            case RubikCubeColor.White: return new Color(1, 1, 1);
            case RubikCubeColor.Red: return new Color(1, 0, 0);
            case RubikCubeColor.Green: return new Color(0, 1, 0);
            case RubikCubeColor.Blue: return new Color(0, 0, 1);
            case RubikCubeColor.Orange: return new Color(1f, 165f / 255f, 0);
            case RubikCubeColor.Yellow: return new Color(1f, 1, 0);
            default:
                throw new Exception("No color define");
        }
    }

    public static RubikCubeFace ConvertPivotToFace(RubikCubePivotable pivot)
    {
        switch (pivot)
        {
            case RubikCubePivotable.Left: return RubikCubeFace.Left;
            case RubikCubePivotable.Right: return RubikCubeFace.Right;
            case RubikCubePivotable.Up: return RubikCubeFace.Up;
            case RubikCubePivotable.Down: return RubikCubeFace.Down;
            case RubikCubePivotable.Face: return RubikCubeFace.Face;
            case RubikCubePivotable.Back: return RubikCubeFace.Back;
            case RubikCubePivotable.Middle:
            case RubikCubePivotable.Equator:
            case RubikCubePivotable.Standing:
            default:
                throw new Exception("Can convert to pivot");
        }
    }
    public static RubikCubePivotable ConvertFaceToPivot(RubikCubeFace pivot)
    {
        switch (pivot)
        {
            case RubikCubeFace.Left: return RubikCubePivotable.Left;
            case RubikCubeFace.Right: return RubikCubePivotable.Right;
            case RubikCubeFace.Up: return RubikCubePivotable.Up;
            case RubikCubeFace.Down: return RubikCubePivotable.Down;
            case RubikCubeFace.Face: return RubikCubePivotable.Face;
            case RubikCubeFace.Back: return RubikCubePivotable.Back;
            default:
                throw new Exception("Can convert to pivot");
        }
    }

    public static string ConvertShortToString(string separation, IEnumerable<RotationTypeShort> seq)
    {
        string sequence = "";
        bool first = true;
        foreach (RotationTypeShort rotation in seq)
        {
            string r = RubikCube.ConvertAcronymShortToString(rotation);
            if (first)
            {
                first = false;
                sequence += r;
            }
            else
            {
                sequence += separation + r;
            }
        }
        return sequence;
    }

}
