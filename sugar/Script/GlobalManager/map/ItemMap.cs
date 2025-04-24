using System;
using System.Collections.Generic;

[System.Serializable]
public class ApartmentBathroom301Map {

}

[System.Serializable]
public class ApartmentCorridorMap
{
    public float wallet;
    public float diary;
    public float pantie;
    public float bra;
    public float trashbag;
}

[System.Serializable]
public class ApartmentElevatorMap {
    
}

[System.Serializable]
public class ApartmentRoom301Map
{
    public float wallet;
    public float diary;
    public float pantie;
    public float bra;
    public float trashbag;
}

[System.Serializable]
public class ItemMap
{
    public ApartmentRoom301Map apartment_room301;
    public ApartmentBathroom301Map apartment_bathroom301;
    public ApartmentCorridorMap apartment_corridor;
    public ApartmentElevatorMap apartment_elevator;
}
