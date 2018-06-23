using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    void Enter();
    void Execute();
    void Exit();
}

public class GS_Title : IGameState
{
    public void Enter() { }
    public void Execute() { }
    public void Exit() { }
}

public class GS_InGame : IGameState
{
    private TetrisController tController;

    public void Enter()
    {
        tController = MonoBehaviour.FindObjectOfType<TetrisController>();

        tController.Init();

        tController.OnFinished = () =>
        {
            // データの保存
            GameManager.Instance.ChangeState(new GS_Result());
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
    public void Enter() { }
    public void Execute() { }
    public void Exit() { }
}