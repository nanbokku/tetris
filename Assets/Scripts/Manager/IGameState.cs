using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Store;

public interface IGameState
{
    void Enter();
    void Execute();
    void Exit();
}

public class GS_Title : IGameState
{
    private TitleUIController uiController;

    public void Enter()
    {
        uiController = MonoBehaviour.FindObjectOfType<TitleUIController>();

        uiController.OnStarted = () =>
        {
            SceneManager.LoadScene("Main");
        };

        // シーン遷移後にゲームステートを更新
        SceneManager.activeSceneChanged += (before, after) =>
        {
            if (before == SceneManager.GetSceneByName("Title"))
            {
                GameManager.Instance.ChangeState(new GS_InGame());
            }
        };
    }

    public void Execute() { }
    public void Exit() { }
}

public class GS_InGame : IGameState
{
    private TetrisController tetrisController;
    private InGameUIControlller gameUIController;

    public void Enter()
    {
        tetrisController = MonoBehaviour.FindObjectOfType<TetrisController>();
        gameUIController = MonoBehaviour.FindObjectOfType<InGameUIControlller>();

        tetrisController.Init();
        gameUIController.Init(StoreManager.Instance);

        tetrisController.OnFinished = () =>
        {
            // データの保存
            GameManager.Instance.ChangeState(new GS_Result());
        };

        gameUIController.OnPauseBtnClicked = () =>
        {
            gameUIController.gameObject.SetActive(false);
        };
    }

    public void Execute() { }
    public void Exit() { }
}

public class GS_Pause : IGameState
{
    public void Enter() { }
    public void Execute() { }
    public void Exit() { }
}

public class GS_Result : IGameState
{
    public void Enter()
    {
        Debug.Log("Enter " + typeof(GS_Result));
    }

    public void Execute() { }
    public void Exit() { }
}