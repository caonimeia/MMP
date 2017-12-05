using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class MFBookInfo {
    public string name;
    public int playerCount;
    public float price;

    public bool free() {
        return price <= 0;
    }

    //public MFBookInfo(string name, int playerCount, float price) {
    //    this.name = name;

    //}
}

public class MFMainView : MFUIBase {
    private MFMainViewBind uiBind;
    private List<MFBookInfo> bookList;

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFMainViewBind>();
        Assert.IsNotNull(uiBind);

        bookList = new List<MFBookInfo>();
        InitBookList();
    }

    protected override void Start() {
        base.Start();


    }

    private void InitBookList() {
        List<MFBookInfo> list = GetBookList();
        using(List<MFBookInfo>.Enumerator itor = list.GetEnumerator()) {
            while (itor.MoveNext()) {
                GameObject bookInfoObj = Instantiate(uiBind.bookTemplate, uiBind.bookListcontentView.transform, false);
                bookInfoObj.SetActive(true);
                Text bookName = MFGameObjectUtil.Find<Text>(bookInfoObj, "Name");
                bookName.text = itor.Current.name;
                Text bookPlayerCount = MFGameObjectUtil.Find<Text>(bookInfoObj, "PlayerCount");
                bookPlayerCount.text = itor.Current.playerCount.ToString();
                Text bookPrice = MFGameObjectUtil.Find<Text>(bookInfoObj, "Price");
                bookPrice.text = itor.Current.free() ? "免费" : itor.Current.price.ToString();
            }
        }
    }

    private List<MFBookInfo> GetBookList() {
        bookList.Clear();
        for (int i = 0; i < 5; i++) {
            MFBookInfo info = new MFBookInfo {
                name = "办公室杀人案" + (i + 1),
                playerCount = 4,
                price = 0
            };
            bookList.Add(info);
        }

        MFBookInfo info2 = new MFBookInfo {
            name = "办公室超级杀人案",
            playerCount = 4,
            price = 2.5f
        };
        bookList.Add(info2);

        return bookList;
    }
}
