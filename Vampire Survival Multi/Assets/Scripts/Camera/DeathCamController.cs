using UnityEngine;

public class DeathCamController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private CameraManager cameraManager;

    // 참조 데이터
    private GameData gameData;

    // 관전 정보
    private LinkedList<GameObject> livingPlayers;
    private Node<GameObject> curPlayer;

    private void Start()
    {
        gameData = GameData.Instance;
    }

    public void OnPlayerDead()
    {
        // Change This Controller
        ControlContext.Instance.SetState(this);

        // Init List
        livingPlayers = InitLivingList();
        curPlayer = livingPlayers.Head;

        if (curPlayer != null && cameraManager != null )
        {
            cameraManager.SetObservePlayer(curPlayer.Value);
        }
    }

    private LinkedList<GameObject> InitLivingList()
    {
        LinkedList<GameObject> result = new LinkedList<GameObject>();

        // 살아있는 플레이어만 목록에 추가
        foreach (GameObject player in gameData.PlayerList)
        {
            if (player != null && player.activeSelf == true)
            {
                result.Add(player);
            }
        }

        return result;
    }

    public void OnPlayerRevive()
    {
        Vector2 revivePos = curPlayer.Value.transform.position;

        // 부활 위치 설정
        LocalPlayerData.Instance.MoveToPos(revivePos);
    }

    /***************************************************************
    * [ 키 입력 ]
    * 
    * 키 입력에 따른 행동 조정
    ***************************************************************/

    public void OnControlKeyPressed()
    {
        OnPreviousKeyPressed();
        OnNextKeyPressed();
    }

    private void OnPreviousKeyPressed()
    {
        if (Input.GetButtonDown("Previous Screen"))
        {
            GameObject player = GetPreviousPlayer();

            if (player != null)
                cameraManager.SetObservePlayer(player);
        }
    }

    private void OnNextKeyPressed()
    {
        if (Input.GetButtonDown("Next Screen"))
        {
            GameObject player = GetNextPlayer();

            if (player != null)
                cameraManager.SetObservePlayer(player);
        }
    }

    private GameObject GetPreviousPlayer()
    {
        // 살아있는 플레이어가 나올 때까지 이전 플레이어 찾기
        curPlayer = curPlayer.Previous;

        while (livingPlayers.Count > 0)
        {
            GameObject player = curPlayer.Value;
            if (player.activeSelf == false)
            {
                // 이전 플레이어가 죽은 상태이면 삭제
                curPlayer = curPlayer.Previous;
                livingPlayers.Remove(player);
            }
            else return player;
        }

        return null;
    }

    private GameObject GetNextPlayer()
    {
        // 살아있는 플레이어가 나올 때까지 다음 플레이어 찾기
        curPlayer = curPlayer.Next;

        while (livingPlayers.Count > 0)
        {
            GameObject player = curPlayer.Value;
            if (player.activeSelf == false)
            {
                // 이전 플레이어가 죽은 상태이면 삭제
                curPlayer = curPlayer.Next;
                livingPlayers.Remove(player);
            }
            else return player;
        }

        return null;
    }

    /***************************************************************
    * [ 연결 리스트 ]
    * 
    * 연결 리스트와 노드 클래스
    ***************************************************************/

    private class Node<T>
    {
        internal T Value;
        internal Node<T> Next;
        internal Node<T> Previous;

        public Node(T data)
        {
            Value = data;
            Next = null;
        }
    }

    private class LinkedList<T>
    {
        private Node<T> _head;
        public Node<T> Head { get { return _head; } }

        private Node<T> _tail;
        public Node<T> Tail { get { return _tail; } }

        private int _count;
        public int Count { get { return _count; } }

        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);

            if (_head == null)
            {
                _head = node;
                _tail = node;

                _head.Next = _tail;
                _tail.Previous = _head;
            }
            else
            {
                node.Next = _head;
                node.Previous = _tail;

                _tail.Next = node;
                _tail = node;

                _head.Previous = _tail;
            }

            _count++;
        }

        public bool Remove(T data)
        {
            Node<T> current = _head;

            if (current == null)
                return false;

            do
            {
                if (current.Value.Equals(data))
                {
                    if (current == _head)
                        _head = _head.Next;

                    if (current == _tail)
                        _tail = _tail.Previous;

                    current.Previous.Next = current.Next;
                    current.Next.Previous = current.Previous;

                    _count--;

                    return true;
                }

                current = current.Next;
            } while (current != _head);

            return false;
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }
    }
}