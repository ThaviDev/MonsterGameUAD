using UnityEngine;

public class DStateMachine : MonoBehaviour
{
    //TODOS LOS ESTADOS QUE NECESITA LA MÁQUINA
    public DS_Blue _stateBlue;
    public DS_Red _stateRed;

    //VARIABLES DE CONTROL DE ESTADO (PARA SABER SI CAMBIO MI ESTADO ACTUAL)
    public DState _curState;
    public DState _lastState;
    public void MyInitial()
    {
        //Inicializar todo lo necesario
        _curState = _stateBlue;
        _lastState = _stateBlue;

        _curState.OnInitial();
    }

    public void MyUpdate()
    {
        // Se actualiza el estado
        _curState = _curState.OnUpdate();


        // Si el estado anterior es diferennte al nuevo
        if (_lastState != _curState)
        {
            // Salir del anterior
            _lastState.OnExit();
            // Iniciar el nuevo
            _curState.OnInitial();
            // Actualizamos el estado anterior para que sea el nuevo
            _lastState = _curState;
        }
    }
}
