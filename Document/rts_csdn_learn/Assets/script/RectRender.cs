using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class RectRender : MonoBehaviour
{

    #region start

    private void Start()
    {
        temp_flag = new GameObject();
        initia_in_start();
        for (int i = 0; i < 10; ++i)
        {
            for (int jj = 0; jj < 10; ++jj)
            {
                Debug.Log(i + " " + jj + " : " + square_offset[i, jj].x + " " + square_offset[i, jj].y);
            }
        }
    }

    #endregion

    #region draw_
    /// <summary>
    /// 这是一个自定义类，用来表示一个生效的选定框；创建它时会自动算出包含四角坐标的四项数据
    /// </summary>
    public class Selector
    {
        public float Xmin;
        public float Xmax;
        public float Ymin;
        public float Ymax;

        //构造函数，在创建选定框时自动计算Xmin/Xmax/Ymin/Ymax
        public Selector(Vector3 start, Vector3 end)
        {
            Xmin = Mathf.Min(start.x, end.x);
            Xmax = Mathf.Max(start.x, end.x);
            Ymin = Mathf.Min(start.y, end.y);
            Ymax = Mathf.Max(start.y, end.y);
        }
    }

    private Selector selector;

    void draw_on_update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            onDrawingRect = true;
            startPoint = Input.mousePosition;
        }

        if (onDrawingRect)
        {
            currentPoint = Input.mousePosition;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            endPoint = Input.mousePosition;
            onDrawingRect = false;
            if (Vector3.Distance(startPoint, endPoint) < 10f)
            {
                ray_shoot();
            }
            else
            {
                selector = new Selector(startPoint, endPoint);
                CheckSelection(selector, "Unit");
            }
        }
    }

    #endregion

    #region 命令切换
    public enum pattern
    {
        move,
        attack,
        cancel
    }
    public pattern mode;
    public void switch_mode(int t)
    {
        mode = (pattern)t;
    }
    void pattern_(GameObject go)
    {
        switch(mode)
        {
            case pattern.move:
                go.GetComponent<base_>().move(temp_flag.transform.position);
                break;
            case pattern.attack:
                go.GetComponent<base_>().attack_arrow(temp_flag.transform.position);
                break;
            case pattern.cancel:

                break;

        }
    }
    void pattern_(GameObject go,Vector3 pos)
    {
        go.GetComponent<base_>().move(pos + temp_flag.transform.position);
    }

    #endregion

    #region 命令动作_未完成
    public struct vec2 { public float x, y; public vec2(float x, float y) { this.x = x; this.y = y; } }
    public vec2[,] square_offset = new vec2[10, 10];
    //float diameter = 1f;
    float gap_between = 2f;
    void ini_square()
    {
        square_offset[0, 0] = new vec2(0f,0f);
        bool turn = false;
        for (int i = 0; i < 10; ++i)
        {
            turn = false;
            if(turn)
            {
                for (int jj = i - 1; jj > -1; --jj) 
                {
                    square_offset[i, jj] = new vec2(square_offset[i, jj - 1].x, square_offset[i, jj - 1].y + 1f);
                }
            }
            for (int jj = 0; jj < 10; ++jj)
            {
                if (i > jj)
                {
                    square_offset[i, jj] = new vec2(square_offset[i - 1,jj].x + 1f, square_offset[i - 1, jj].y);
                }
                else if (i == jj)
                {
                    if (i == 0) continue;
                    square_offset[i, jj] = new vec2(square_offset[i - 1, jj - 1].x + 1f, square_offset[i - 1, jj - 1].y + 1f);
                    turn = true;
                }
                else //(i < jj)
                {
                    square_offset[i, jj] = new vec2(square_offset[i, jj -1 ].x, square_offset[i, jj -1 ].y + 1f);
                }
            }
        }
    }

    void initia_in_start()
    {
        ini_square();
    }
    #endregion

    #region select_unit___shoot
    public List<GameObject> unit_select = new List<GameObject>();

    public GameObject flag;
    public GameObject temp_flag;
    public GameObject find_the_ray;
    void ray_shoot()
    {
        if (CheckGuiRaycastObjects()) return;
        //Debug.Log("start ray");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 3f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50000, 1 << 0, QueryTriggerInteraction.Ignore)) 
        {
            find_the_ray = hit.collider.gameObject;

            if (temp_flag != null) Destroy(temp_flag);
            //Debug.Log("检测到物体");
            temp_flag = Instantiate(flag, hit.point, Quaternion.identity);
        }
        Vector3 offset = new Vector3(0, 0, 0);//temp test
        foreach (GameObject go in unit_select)
        {
            if(go==null)
            {
                unit_select.Clear();
                break;
            }
            if(mode == pattern.move)
            {
                pattern_(go, offset);
                offset += Vector3.forward * 2; // 暂时用的队列
                continue;
            }
            pattern_(go);
        }
    }
    public EventSystem EventSystem_;
    public BaseRaycaster caster;
    bool CheckGuiRaycastObjects()
    {
        PointerEventData eventData = new PointerEventData(EventSystem_);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        caster.Raycast(eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }


    void CheckSelection(Selector selector, string tag)
    {
        foreach (GameObject go in unit_select)
        {
            go.GetComponent<base_>().switch_select();
        }
        unit_select.Clear();
        GameObject[] Units = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject Unit in Units)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Unit.transform.position);
            if (screenPos.x > selector.Xmin && screenPos.x < selector.Xmax && screenPos.y > selector.Ymin && screenPos.y < selector.Ymax)
            {
                //Debug.LogFormat("已选中目标:{0}", Unit.name);
                Unit.GetComponent<base_>().switch_select();
                unit_select.Add(Unit);
            }

            Debug.Log(unit_select.Count);
            foreach(GameObject t in unit_select)
            {
                Debug.Log(t.name+" ");
            }
        }
    }
    #endregion

    #region 绘制
    public Material GLRectMat;//绘图的材质，在Inspector中设置
    public Color GLRectColor;//矩形的内部颜色，在Inspector中设置
    public Color GLRectEdgeColor;//矩形的边框颜色，在Inspector中设置
    private bool onDrawingRect;//是否正在画框(即鼠标左键处于按住的状态)

    private Vector3 startPoint;//框的起始点，即按下鼠标左键时指针的位置
    private Vector3 currentPoint;//在拖移过程中，玩家鼠标指针所在的实时位置
    private Vector3 endPoint;//框的终止点，即放开鼠标左键时指针的位置
    void OnPostRender()
    {
        if (onDrawingRect)
        {
            //准备工作:获取确定矩形框各角坐标所需的各个数值
            float Xmin = Mathf.Min(startPoint.x, currentPoint.x);
            float Xmax = Mathf.Max(startPoint.x, currentPoint.x);
            float Ymin = Mathf.Min(startPoint.y, currentPoint.y);
            float Ymax = Mathf.Max(startPoint.y, currentPoint.y);

            GL.PushMatrix();//GL入栈
                            //在这里，只需要知道GL.PushMatrix()和GL.PopMatrix()分别是画图的开始和结束信号,画图指令写在它们中间
            if (!GLRectMat)
                return;

            GLRectMat.SetPass(0);//启用线框材质rectMat

            GL.LoadPixelMatrix();//设置用屏幕坐标绘图

            /*------第一步，绘制矩形------*/
            GL.Begin(GL.QUADS);//开始绘制矩形,这一段画的是框中间的半透明部分，不包括边界线

            GL.Color(GLRectColor);//设置矩形的颜色，注意GLRectColor务必设置为半透明

            //陈述矩形的四个顶点
            GL.Vertex3(Xmin, Ymin, 0);//陈述第一个点，即框的左下角点，记为点1
            GL.Vertex3(Xmin, Ymax, 0);//陈述第二个点，即框的左上角点，记为点2
            GL.Vertex3(Xmax, Ymax, 0);//陈述第三个点，即框的右上角点，记为点3
            GL.Vertex3(Xmax, Ymin, 0);//陈述第四个点，即框的右下角点，记为点4

            GL.End();//告一段落，此时画好了一个无边框的矩形


            /*------第二步，绘制矩形的边框------*/
            GL.Begin(GL.LINES);//开始绘制线，用来描出矩形的边框

            GL.Color(GLRectEdgeColor);//设置方框的边框颜色，建议设置为不透明的

            //描第一条边
            GL.Vertex3(Xmin, Ymin, 0);//起始于点1
            GL.Vertex3(Xmin, Ymax, 0);//终止于点2

            //描第二条边
            GL.Vertex3(Xmin, Ymax, 0);//起始于点2
            GL.Vertex3(Xmax, Ymax, 0);//终止于点3

            //描第三条边
            GL.Vertex3(Xmax, Ymax, 0);//起始于点3
            GL.Vertex3(Xmax, Ymin, 0);//终止于点4

            //描第四条边
            GL.Vertex3(Xmax, Ymin, 0);//起始于点4
            GL.Vertex3(Xmin, Ymin, 0);//返回到点1

            GL.End();//画好啦！

            GL.PopMatrix();//GL出栈
        }
    }
    #endregion

    #region instance
    public static RectRender ins;
    private void Awake()
    {
        ins = this;
    }

    #endregion

    void Update()
    {
        draw_on_update();
    }
}