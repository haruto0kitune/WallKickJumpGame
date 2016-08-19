using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using NCMB;

public class RankingManager : MonoBehaviour
{
    [SerializeField]
    Text scoreFirstName;
    [SerializeField]
    Text scoreFirstScore;
    [SerializeField]
    Text scoreSecondName;
    [SerializeField]
    Text scoreSecondScore;
    [SerializeField]
    Text scoreThirdName;
    [SerializeField]
    Text scoreThirdScore;
    [SerializeField]
    Text meterFirstName;
    [SerializeField]
    Text meterFirstMeter;
    [SerializeField]
    Text meterSecondName;
    [SerializeField]
    Text meterSecondMeter;
    [SerializeField]
    Text meterThirdName;
    [SerializeField]
    Text meterThirdMeter;
    [SerializeField]
    Text playerScoreRank;
    [SerializeField]
    Text playerScoreScore;
    [SerializeField]
    Text playerMeterRank;
    [SerializeField]
    Text playerMeterMeter;

    void Start()
    {
        NCMBQuery<NCMBObject> scoreRankingQuery = new NCMBQuery<NCMBObject>("scoreRanking");
        NCMBQuery<NCMBObject> meterRankingQuery = new NCMBQuery<NCMBObject>("meterRanking");

        scoreRankingQuery.OrderByDescending("score");
        meterRankingQuery.OrderByDescending("meter");

        scoreRankingQuery.Limit = 3;
        meterRankingQuery.Limit = 3;

        double floorConstantValue = 100;

        scoreRankingQuery.Find((List<NCMBObject> objList, NCMBException e) =>
        {
            if(e != null)
            {

            }
            else
            {
                //List<string> nameList = new List<string>();

                //foreach (var item in objList)
                //{
                //    nameList.Add(item["name"].ToString());
                //}

                scoreFirstName.text = System.Convert.ToString(objList[0]["name"]);
                scoreFirstScore.text = System.Convert.ToString(objList[0]["score"]) + " point";
                scoreSecondName.text = System.Convert.ToString(objList[1]["name"]);
                scoreSecondScore.text = System.Convert.ToString(objList[1]["score"]) + " point";
                scoreThirdName.text = System.Convert.ToString(objList[2]["name"]);
                scoreThirdScore.text = System.Convert.ToString(objList[2]["score"]) + " point";
            }
        });

        meterRankingQuery.Find((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {

            }
            else
            {
                meterFirstName.text = System.Convert.ToString(objList[0]["name"]);
                meterFirstMeter.text = System.Convert.ToString((System.Convert.ToInt16(System.Convert.ToDouble(objList[0]["meter"]) * floorConstantValue)) / floorConstantValue) + " m";
                meterSecondName.text = System.Convert.ToString(objList[1]["name"]);
                meterSecondMeter.text = System.Convert.ToString((System.Convert.ToInt16(System.Convert.ToDouble(objList[1]["meter"]) * floorConstantValue)) / floorConstantValue) + " m";
                meterThirdName.text = System.Convert.ToString(objList[2]["name"]);
                meterThirdMeter.text = System.Convert.ToString((System.Convert.ToInt16(System.Convert.ToDouble(objList[2]["meter"]) * floorConstantValue)) / floorConstantValue) + " m";
            }
        });

        NCMBQuery<NCMBObject> playerScoreRanking = NCMBQuery<NCMBObject>.GetQuery("scoreRanking");
        NCMBQuery<NCMBObject> playerMeterRanking = NCMBQuery<NCMBObject>.GetQuery("meterRanking");

        playerScoreRanking.WhereEqualTo("uuid", PlayerPrefs.GetString("uuid"));
        playerMeterRanking.WhereEqualTo("uuid", PlayerPrefs.GetString("uuid"));

        playerScoreRanking.OrderByDescending("score");
        playerMeterRanking.OrderByDescending("meter");

        playerScoreRanking.Find((List<NCMBObject> objList, NCMBException e) =>
        {
            if(e != null)
            {

            }
            else
            {
                playerScoreScore.text = System.Convert.ToString(objList[0]["score"]) + " point";
            }
        });

        playerMeterRanking.Find((List<NCMBObject> objList, NCMBException e) =>
        {
            if(e != null)
            {

            }
            else
            {
                playerMeterMeter.text = System.Convert.ToString((System.Convert.ToInt16(System.Convert.ToDouble(objList[0]["meter"]) * floorConstantValue)) / floorConstantValue) + " m";
            }
        });
    }
}
