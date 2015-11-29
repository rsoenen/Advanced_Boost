using UnityEngine;
using System.Collections;

public class Championnat {

    private int idChampionnat;
    private ArrayList pointChampionnat;
    private ArrayList participants;
    

    public Championnat(ArrayList _participants, int _id){
        this.idChampionnat=_id;
        this.participants = _participants;


        pointChampionnat=new ArrayList();
        for (int i = 0; i < _participants.Count; i++){
            pointChampionnat.Add(0);
        }
    }
    

}
