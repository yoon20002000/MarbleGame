using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_MarbleDataInput : MonoBehaviour
{
   // [SerializeField]
   // private GameManager gameManager;

   [Header("UI")]
   [Header("InputField")]
   [SerializeField]
   private TMP_InputField marbleNameInputField;
   [SerializeField]
   private TMP_InputField marbleCountInputField;
   
   [Header("Buttons")]
   [SerializeField]
   private Button addMarbleButton;
   [SerializeField]
   private Button removeMarbleButton;
   [SerializeField]
   private Button resetMarbleButton;
   private void Awake()
   {
     
      MarbleGameManager.Instance.OnGameStateChanged.AddListener(OnRacingStateChanged);
      
      addMarbleButton.onClick.AddListener(OnClickAddMarbleButton);
      removeMarbleButton.onClick.AddListener(OnClickRemoveMarbleButton);
      resetMarbleButton.onClick.AddListener(OnClickResetMarbleButton);
   }

   private void OnDestroy()
   {
      MarbleGameManager.Instance.OnGameStateChanged.RemoveListener(OnRacingStateChanged);
   }

   private void OnClickAddMarbleButton()
   {
      int marbleCount = int.Parse(marbleCountInputField.text);
      MarbleGameManager.Instance.AddMarble(marbleNameInputField.text, marbleCount);
   }

   private void OnClickRemoveMarbleButton()
   {
      int marbleCount = int.Parse(marbleCountInputField.text);
      MarbleGameManager.Instance.RemoveMarble(marbleNameInputField.text, marbleCount);
   }

   private void OnClickResetMarbleButton()
   {
      
   }

   private void OnRacingStateChanged(MarbleGameManager.EGameState gameState)
   {
      bool isUIInteractable =
         gameState != MarbleGameManager.EGameState.Racing && gameState != MarbleGameManager.EGameState.RacingEnd;
      addMarbleButton.interactable = isUIInteractable;
      removeMarbleButton.interactable = isUIInteractable;
      resetMarbleButton.interactable = isUIInteractable;
   }
}
