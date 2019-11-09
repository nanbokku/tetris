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
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    public void Execute() { }

    public void Exit()
    {
        // イベントの削除
        uiController.OnStarted = null;

        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene before, Scene after)
    {
        if (after == SceneManager.GetSceneByName("Main"))
        {
            GameManager.Instance.ChangeState(new GS_InGame());
        }
    }
}

public class GS_InGame : IGameState
{
    private TetrisController tetrisController;
    private InGameUIController gameUIController;

    public void Enter()
    {
        tetrisController = MonoBehaviour.FindObjectOfType<TetrisController>();
        gameUIController = MonoBehaviour.FindObjectOfType<InGameUIController>();

        tetrisController.Init();
        gameUIController.Init(StoreManager.Instance);

        tetrisController.OnFinished = () =>
        {
            // TODO: データの保存
            SceneManager.LoadScene("Result");
        };

        // シーン遷移後にステートを変更
        SceneManager.activeSceneChanged += OnSceneChanged;

        gameUIController.OnPaused = () =>
    {
    };

        gameUIController.OnInGamed = () =>
        {

        };

        gameUIController.OnTitled = () =>
        {

        };
    }

    public void Execute() { }

    public void Exit()
    {
        gameUIController.Exit();

        // イベントの削除
        tetrisController.OnFinished = null;
        gameUIController.OnPaused = null;
        gameUIController.OnInGamed = null;
        gameUIController.OnTitled = null;

        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene before, Scene after)
    {
        if (after == SceneManager.GetSceneByName("Result"))
        {
            GameManager.Instance.ChangeState(new GS_Result());
        }
    }
}

public class GS_Pause : IGameState
{
    public void Enter() { }
    public void Execute() { }
    public void Exit() { }
}

public class GS_Result : IGameState
{
    private ResultUIController uIController = null;

    public void Enter()
    {
        Debug.Log("Enter " + typeof(GS_Result));

        uIController = MonoBehaviour.FindObjectOfType<ResultUIController>();

        uIController.OnTitleBtnClicked = () =>
        {
            SceneManager.LoadScene("Title");
        };

        SceneManager.activeSceneChanged += OnSceneChanged;

        uIController.Init(StoreManager.Instance);
    }

    public void Execute() { }

    public void Exit()
    {
        StoreManager.Instance.Init();

        uIController.OnTitleBtnClicked = null;

        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene before, Scene after)
    {
        if (after == SceneManager.GetSceneByName("Title"))
        {
            GameManager.Instance.ChangeState(new GS_Title());
        }
    }
}