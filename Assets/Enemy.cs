using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// �G�𐧌䂷��
/// </summary>
public class Enemy : MapObjectBase
{
    [Serializable]
    public class Scope
    {
        // South����
        [TextArea(3, 10)]
        public string area = ""
            + "111\n"
            + "111\n"
            + "111";

        public bool IsInArea(Vector2Int target, Vector2Int startPos, Direction dir)
        {
            var relativePos = target - startPos;
            switch (dir)
            {
                case Direction.North:
                    relativePos.x *= -1;
                    relativePos.y *= -1;
                    break;
                case Direction.South:
                    break;
                case Direction.East:
                    var tmp = relativePos.x;
                    relativePos.x = -relativePos.y;
                    relativePos.y = tmp;
                    break;
                case Direction.West:
                    tmp = relativePos.x;
                    relativePos.x = relativePos.y;
                    relativePos.y = -tmp;
                    break;
            }

            var lines = area.Split('\n');
            var width = lines.Select(_l => _l.Length).FirstOrDefault();
            if(!lines.All(_l => _l.Length == width))
            {
                throw new Exception("Area�̊e�s�ɃT�C�Y���قȂ���̂����݂��Ă��܂�");
            }

            var left = -width / 2;
            var right = left + width;
            if(left <= relativePos.x && relativePos.x < right)
            {
                if (1 <= relativePos.y && relativePos.y <= lines.Length)
                {
                    var offsetX = relativePos.x - left;
                    if ('1' == lines[relativePos.y - 1][offsetX])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public bool _isChasing = false;
    public Scope _visibleArea;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void MoveStart()
    {
        var player = FindObjectOfType<Player>();
        if (!MoveToFollow(player))
        {
            MoveFree();
        }
    }

    protected void MoveFree()
    {
        // ���݂̍��������珇�ɉE���ɐi�߂�}�X���m�F���Ă����B
        var startDir = Map.TurnLeftDirection(_forward);
        _forward = startDir;
        do
        {
            // �ǂ��ƈړ��ł��Ȃ�
            // ���ɂق��̃}�b�v�I�u�W�F�N�g�����݂��Ă���ꍇ�͈ړ��ł��Ȃ�
            var (movedMass, movedPos) = Map.GetMovePos(_pos, _forward);
            var massData = movedMass == null ? null : Map[movedMass.type];
            if (movedMass == null || movedMass.existObject != null || !massData.isRoad)
            {
                // �ړ��ł��Ȃ���Ό�����ς��A�ړ��m�F���s��
                _forward = Map.TurnRightDirection(_forward);
            }
            else
            {
                break;
            }
        } while (startDir != _forward);

        // �ړ��̑O���������肵����ړ�����
        Move(_forward);
    }

    protected bool MoveToFollow(MapObjectBase target)
    {
        if (_visibleArea.IsInArea(target._pos, _pos, _forward))
        {
            Move(_forward);
            _isChasing = true;
            return true;
        }

        if (_isChasing)
        {
            var left = Map.TurnLeftDirection(_forward);
            if (_visibleArea.IsInArea(target._pos, _pos, left))
            {
                Move(_forward);
                _forward = left;
                _isChasing = true;
                return true;
            }
            var right = Map.TurnRightDirection(_forward);
            if (_visibleArea.IsInArea(target._pos, _pos, right))
            {
                Move(_forward);
                _forward = right;
                _isChasing = true;
                return true;
            }
        }

        _isChasing = false;
        return false;
    }
}
