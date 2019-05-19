
#region RUBIK CUBE ENUM
public enum ArrowDirection3D { Face, Right, Back, Left, Up, Down }
public enum FullCubeRotation { X, Xi, Y, Yi, Z, Zi }
public enum RubikCubeDepth : int { Face = 0, Standing = 1, Back = 2 }
public enum ArrowDirection { Left, Right, Up, Down }
public enum RotationTypeShort
{
    L, Lp,
    R, Rp,
    U, Up,
    D, Dp,
    F, Fp,
    B, Bp,
    M, Mp,
    E, Ep,
    S, Sp
}
public enum RotationTypeLong
{
    Left, LeftCounter,
    Right, RightCounter,
    Up, UpCounter,
    Down, DownCounter,
    Face, FaceCounter,
    Back, BackCounter,
    Middle, MiddleCounter,
    Equator, EquatorCounter,
    Standing, StandingCounter
}
public enum RubikCubePivotable : int
{
    Left,
    Right,
    Up,
    Down,
    Face,
    Back,
    Middle,
    Equator,
    Standing

}
public enum Axis { X, Y, Z }
public enum RotationType { Vertical, Horizontal, Lateral }
public enum RubikCubeFace : int
{
    Left,
    Right,
    Up,
    Down,
    Face,
    Back

}
public enum RubikCubeFaceDirection : int
{

    SO = 1,
    S = 2,
    SE = 3,
    O = 4,
    C = 5,
    E = 6,
    NO = 7,
    N = 8,
    NE = 9
}
public enum RubikPieceByPosition3D : int
{
    X0_Y0_Z0 = 0,
    X1_Y0_Z0 = 1,
    X2_Y0_Z0 = 2,
    X0_Y0_Z1 = 3,
    X1_Y0_Z1 = 4,
    X2_Y0_Z1 = 5,
    X0_Y0_Z2 = 6,
    X1_Y0_Z2 = 7,
    X2_Y0_Z2 = 8,
    X0_Y1_Z0 = 9,
    X1_Y1_Z0 = 10,
    X2_Y1_Z0 = 11,
    X0_Y1_Z1 = 12,
    X1_Y1_Z1 = 13,
    X2_Y1_Z1 = 14,
    X0_Y1_Z2 = 15,
    X1_Y1_Z2 = 16,
    X2_Y1_Z2 = 17,
    X0_Y2_Z0 = 18,
    X1_Y2_Z0 = 19,
    X2_Y2_Z0 = 20,
    X0_Y2_Z1 = 21,
    X1_Y2_Z1 = 22,
    X2_Y2_Z1 = 23,
    X0_Y2_Z2 = 24,
    X1_Y2_Z2 = 25,
    X2_Y2_Z2 = 26


}
public enum RubikPiecePositionByPivot : int
{

    M_E_S = -1,
    L_D_F = 0,
    L_D_S = 1,
    L_D_B = 2,
    L_E_F = 3,
    L_E_S = 4,
    L_E_B = 5,
    L_U_F = 6,
    L_U_S = 7,
    L_U_B = 8,

    M_D_F = 9,
    M_D_S = 10,
    M_D_B = 11,
    M_E_F = 12,
    M_E_B = 13,
    M_U_F = 14,
    M_U_S = 15,
    M_U_B = 16,

    R_D_F = 17,
    R_D_S = 18,
    R_D_B = 19,
    R_E_F = 20,
    R_E_S = 21,
    R_E_B = 22,
    R_U_F = 23,
    R_U_S = 24,
    R_U_B = 25,
}
public enum RubikPieceType : int
{

    X2_Y2_Z2 = -1,
    X1_Y1_Z1 = 0,
    X1_Y1_Z2 = 1,
    X1_Y1_Z3 = 2,
    X1_Y2_Z1 = 3,
    X1_Y2_Z2 = 4,
    X1_Y2_Z3 = 5,
    X1_Y3_Z1 = 6,
    X1_Y3_Z2 = 7,
    X1_Y3_Z3 = 8,

    X2_Y1_Z1 = 9,
    X2_Y1_Z2 = 10,
    X2_Y1_Z3 = 11,
    X2_Y2_Z1 = 12,
    X2_Y2_Z3 = 13,
    X2_Y3_Z1 = 14,
    X2_Y3_Z2 = 15,
    X2_Y3_Z3 = 16,

    X3_Y1_Z1 = 17,
    X3_Y1_Z2 = 18,
    X3_Y1_Z3 = 19,
    X3_Y2_Z1 = 20,
    X3_Y2_Z2 = 21,
    X3_Y2_Z3 = 22,
    X3_Y3_Z1 = 23,
    X3_Y3_Z2 = 24,
    X3_Y3_Z3 = 25,
}
public enum RubikCubeColor
{
    White, Red, Green, Blue, Orange, Yellow

}
public enum EquatorDirection
{
    Equator, Middle, Standing, CounterEquator, CounterMiddle, CounterStanding
}
#endregion
