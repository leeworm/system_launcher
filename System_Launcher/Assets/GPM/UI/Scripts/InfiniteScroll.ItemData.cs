// GPM UI 네임스페이스 선언
namespace Gpm.Ui
{
    // 시스템 관련 라이브러리 사용
    using System;
    using System.Collections.Generic;
    using UnityEngine.Events;

    // InfiniteScroll 클래스의 부분 클래스 정의
    public partial class InfiniteScroll
    {
        // 데이터 컨텍스트 클래스 - 각 아이템의 데이터와 상태를 관리
        public class DataContext
        {
            // 데이터 컨텍스트 생성자 - 데이터와 인덱스를 받아 초기화
            public DataContext(InfiniteScrollData data, int index)
            {
                // 인덱스 설정
                this.index = index;
                // 데이터 설정
                this.data = data;
            }

            // 읽기 전용 데이터 프로퍼티
            public InfiniteScrollData data { get; private set; }

            // 데이터 리스트에서의 인덱스 (-1로 초기화)
            internal int index = -1;

            // 아이템 리스트에서의 인덱스 (-1로 초기화)
            internal int itemIndex = -1;

            // 스크롤 오프셋 값
            internal float offset = 0;

            // 아이템 데이터 업데이트 필요 여부 플래그
            internal bool needUpdateItemData = true;

            // 스크롤 아이템의 크기
            internal float scrollItemSize = 0;

            // 실제 아이템 오브젝트 참조
            internal InfiniteScrollItem itemObject;

            // 아이템 데이터 업데이트가 필요한지 확인하는 메서드
            public bool IsNeedUpdateItemData()
            {
                // 업데이트 필요 플래그 반환
                return needUpdateItemData;
            }

            // 아이템과의 연결을 해제하는 메서드
            public void UnlinkItem(bool notifyEvent = false)
            {
                // 아이템 오브젝트가 존재하는 경우
                if (itemObject != null)
                {
                    // 아이템 데이터 클리어 (이벤트 알림 여부 설정)
                    itemObject.ClearData(notifyEvent);
                    // 아이템 오브젝트 참조 해제
                    itemObject = null;
                }

                // 아이템 인덱스를 -1로 리셋
                itemIndex = -1;
            }

            // 데이터를 업데이트하는 메서드
            public void UpdateData(InfiniteScrollData data)
            {
                // 새로운 데이터로 교체
                this.data = data;
                // 데이터 업데이트 필요 플래그 설정
                needUpdateItemData = true;
            }

            // 아이템 크기를 반환하는 메서드
            public float GetItemSize()
            {
                // 스크롤 아이템 크기 반환
                return scrollItemSize;
            }

            // 아이템 크기를 설정하는 메서드
            public void SetItemSize(float value)
            {
                // 스크롤 아이템 크기 설정
                scrollItemSize = value;
            }
        }

        // 데이터 컨텍스트 리스트 - 모든 데이터를 관리
        protected List<DataContext> dataList = new List<DataContext>();
        // 현재 아이템 개수
        protected int itemCount = 0;

        // 아이템 리스트 업데이트 필요 여부 플래그
        protected bool needUpdateItemList = true;

        // 현재 선택된 데이터의 인덱스
        protected int selectDataIndex = -1;
        // 선택 시 호출될 콜백 함수
        protected Action<InfiniteScrollData> selectCallback = null;

        // 특정 데이터의 인덱스를 찾는 메서드
        public int GetDataIndex(InfiniteScrollData data)
        {
            // 초기화가 안된 경우 초기화 실행
            if (isInitialize == false)
            {
                Initialize();
            }

            // 데이터 리스트에서 해당 데이터의 인덱스 찾아서 반환
            return dataList.FindIndex((context) =>
            {
                // 데이터가 일치하는지 확인
                return context.data.Equals(data);
            });
        }

        // 전체 데이터 개수를 반환하는 메서드
        public int GetDataCount()
        {
            // 데이터 리스트의 개수 반환
            return dataList.Count;
        }

        // 특정 인덱스의 데이터를 반환하는 메서드
        public InfiniteScrollData GetData(int index)
        {
            // 해당 인덱스의 데이터 반환
            return dataList[index].data;
        }

        // 전체 데이터 리스트를 반환하는 메서드
        public List<InfiniteScrollData> GetDataList()
        {
            // 새로운 데이터 리스트 생성
            List<InfiniteScrollData> list = new List<InfiniteScrollData>();

            // 모든 데이터 컨텍스트를 순회
            for (int index = 0; index < dataList.Count; index++)
            {
                // 데이터만 추출해서 리스트에 추가
                list.Add(dataList[index].data);
            }
            // 완성된 리스트 반환
            return list;
        }

        // 현재 표시되는 아이템들의 데이터 리스트를 반환하는 메서드
        public List<InfiniteScrollData> GetItemList()
        {
            // 새로운 아이템 리스트 생성
            List<InfiniteScrollData> list = new List<InfiniteScrollData>();

            // 모든 데이터 컨텍스트를 순회
            for (int index = 0; index < dataList.Count; index++)
            {
                // 아이템 인덱스가 유효한 경우만 (실제 표시되는 아이템)
                if (dataList[index].itemIndex != -1)
                {
                    // 해당 데이터를 리스트에 추가
                    list.Add(dataList[index].data);
                }
            }
            // 완성된 아이템 리스트 반환
            return list;
        }

        // 현재 아이템 개수를 반환하는 메서드
        public int GetItemCount()
        {
            // 아이템 개수 반환
            return itemCount;
        }

        // 특정 데이터의 아이템 인덱스를 반환하는 메서드
        public int GetItemIndex(InfiniteScrollData data)
        {
            // 해당 데이터의 컨텍스트 찾기
            var context = GetDataContext(data);
            // 아이템 인덱스 반환
            return context.itemIndex;
        }

        // 선택 콜백 함수를 추가하는 메서드
        public void AddSelectCallback(Action<InfiniteScrollData> callback)
        {
            // 초기화가 안된 경우 초기화 실행
            if (isInitialize == false)
            {
                Initialize();
            }

            // 콜백 함수 추가
            selectCallback += callback;
        }

        // 선택 콜백 함수를 제거하는 메서드
        public void RemoveSelectCallback(Action<InfiniteScrollData> callback)
        {
            // 초기화가 안된 경우 초기화 실행
            if (isInitialize == false)
            {
                Initialize();
            }

            // 콜백 함수 제거
            selectCallback -= callback;
        }

        // 아이템의 활성 상태 변경 시 호출되는 메서드
        public void OnChangeActiveItem(int dataIndex, bool active)
        {
            // 활성 상태 변경 이벤트 발생
            onChangeActiveItem.Invoke(dataIndex, active);
        }

        // 특정 데이터의 컨텍스트를 찾는 메서드
        protected DataContext GetDataContext(InfiniteScrollData data)
        {
            // 초기화가 안된 경우 초기화 실행
            if (isInitialize == false)
            {
                Initialize();
            }

            // 데이터 리스트에서 해당 데이터의 컨텍스트 찾아서 반환
            return dataList.Find((context) =>
            {
                // 데이터가 일치하는지 확인
                return context.data.Equals(data);
            });
        }

        // 아이템 인덱스로 컨텍스트를 찾는 메서드
        protected DataContext GetContextFromItem(int itemIndex)
        {
            // 초기화가 안된 경우 초기화 실행
            if (isInitialize == false)
            {
                Initialize();
            }

            // 아이템 인덱스가 유효한 경우
            if (IsValidItemIndex(itemIndex) == true)
            {
                // 해당 아이템 반환
                return GetItem(itemIndex);
            }
            else
            {
                // 유효하지 않은 경우 null 반환
                return null;
            }
        }

        // 데이터를 추가하는 메서드
        protected void AddData(InfiniteScrollData data)
        {
            // 새로운 데이터 컨텍스트 생성 (인덱스는 현재 리스트 크기)
            DataContext addData = new DataContext(data, dataList.Count);
            // 컨텍스트 초기화
            InitFitContext(addData);

            // 데이터 리스트에 추가
            dataList.Add(addData);

            // 데이터 추가 후 아이템 체크
            CheckItemAfterAddData(addData);
        }

        // 데이터 추가 후 아이템 상태를 체크하는 메서드
        private bool CheckItemAfterAddData(DataContext addData)
        {
            // 필터가 있고 필터링 된 경우
            if (onFilter != null &&
                onFilter(addData.data) == true)
            {
                // 처리 중단
                return false;
            }

            // 새로운 아이템의 인덱스 계산 (기본값 0)
            int itemIndex = 0;
            // 기존 아이템이 있는 경우
            if (itemCount > 0)
            {
                // 추가된 데이터 이전의 데이터들을 역순으로 순회
                for (int dataIndex = addData.index - 1; dataIndex >= 0; dataIndex--)
                {
                    // 유효한 아이템 인덱스를 가진 데이터를 찾으면
                    if (dataList[dataIndex].itemIndex != -1)
                    {
                        // 그 다음 인덱스를 새 아이템 인덱스로 설정
                        itemIndex = dataList[dataIndex].itemIndex + 1;
                        break;
                    }
                }
            }

            // 새 데이터에 아이템 인덱스 설정
            addData.itemIndex = itemIndex;
            // 전체 아이템 개수 증가
            itemCount++;

            // 추가된 데이터 이후의 모든 데이터들의 아이템 인덱스 증가
            for (int dataIndex = addData.index + 1; dataIndex < dataList.Count; dataIndex++)
            {
                // 유효한 아이템 인덱스를 가진 데이터만 증가
                if (dataList[dataIndex].itemIndex != -1)
                {
                    dataList[dataIndex].itemIndex++;
                }
            }

            // 레이아웃 재구성 필요 플래그 설정
            needReBuildLayout = true;

            // 성공적으로 처리됨
            return true;
        }

        // 특정 위치에 데이터를 삽입하는 메서드
        protected void InsertData(InfiniteScrollData data, int insertIndex)
        {
            // 삽입 인덱스가 유효하지 않은 경우 예외 발생
            if (insertIndex < 0 || insertIndex > dataList.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            // 리스트 중간에 삽입하는 경우
            if (insertIndex < dataList.Count)
            {
                // 새로운 데이터 컨텍스트 생성
                DataContext addData = new DataContext(data, insertIndex);
                // 컨텍스트 초기화
                InitFitContext(addData);

                // 삽입 위치 이후의 모든 데이터들의 인덱스 증가
                for (int dataIndex = insertIndex; dataIndex < dataList.Count; dataIndex++)
                {
                    dataList[dataIndex].index++;
                }

                // 지정된 위치에 데이터 삽입
                dataList.Insert(insertIndex, addData);

                // 데이터 추가 후 아이템 체크
                CheckItemAfterAddData(addData);
            }
            else
            {
                // 리스트 끝에 추가하는 경우 AddData 메서드 호출
                AddData(data);
            }
        }

        // 컨텍스트를 초기화하는 메서드
        protected void InitFitContext(DataContext context)
        {
            // 기본 아이템 크기에서 메인 크기 가져오기
            float size = layout.GetMainSize(defaultItemPrefabSize);
            // 동적 아이템 크기 사용하는 경우
            if (dynamicItemSize == true)
            {
                // 컨텍스트에서 아이템 크기 가져오기
                float ItemSize = context.GetItemSize();
                // 아이템 크기가 설정되어 있는 경우
                if (ItemSize != 0)
                {
                    // 해당 크기 사용
                    size = ItemSize;
                }
            }

            // 컨텍스트에 아이템 크기 설정
            context.SetItemSize(size);
        }

        // 데이터 인덱스가 유효한지 확인하는 메서드
        protected bool IsValidDataIndex(int index)
        {
            // 인덱스가 0 이상이고 데이터 리스트 크기 미만인지 확인
            return (index >= 0 && index < dataList.Count) ? true : false;
        }

        // 아이템 인덱스가 유효한지 확인하는 메서드
        protected bool IsValidItemIndex(int index)
        {
            // 인덱스가 0 이상이고 아이템 개수 미만인지 확인
            return (index >= 0 && index < itemCount) ? true : false;
        }

        // 아이템 리스트를 구성하는 메서드
        protected void BuildItemList()
        {
            // 아이템 개수 초기화
            itemCount = 0;
            // 모든 데이터 컨텍스트를 순회
            for (int i = 0; i < dataList.Count; i++)
            {
                // 현재 데이터 컨텍스트 가져오기
                DataContext context = dataList[i];

                // 필터가 있고 필터링 된 경우
                if (onFilter != null &&
                     onFilter(context.data) == true)
                {
                    // 아이템 연결 해제 (이벤트 알림 없이)
                    context.UnlinkItem(false);

                    // 다음 아이템으로 넘어감
                    continue;
                }
                // 유효한 아이템에 인덱스 할당
                context.itemIndex = itemCount;
                // 아이템 개수 증가
                itemCount++;
            }

            // 레이아웃 재구성 필요 플래그 설정
            needReBuildLayout = true;
        }

        // 아이템 선택 시 호출되는 메서드
        private void OnSelectItem(InfiniteScrollData data)
        {
            // 선택된 데이터의 인덱스 찾기
            int dataIndex = GetDataIndex(data);
            // 데이터 인덱스가 유효한 경우
            if (IsValidDataIndex(dataIndex) == true)
            {
                // 선택된 데이터 인덱스 저장
                selectDataIndex = dataIndex;

                // 선택 콜백이 있는 경우
                if (selectCallback != null)
                {
                    // 콜백 함수 호출
                    selectCallback(data);
                }
            }
        }

        // 데이터 리스트를 정렬하는 메서드
        public void SortDataList(Comparison<DataContext> comparison)
        {
            // 주어진 비교 함수로 데이터 리스트 정렬
            dataList.Sort(comparison);

            // 아이템 리스트 업데이트 필요 플래그 설정
            needUpdateItemList = true;
            // 표시되는 아이템 업데이트
            UpdateShowItem();
        }
    }
}