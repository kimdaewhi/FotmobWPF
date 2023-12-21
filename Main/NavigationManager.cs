using System;
using System.Collections.Generic;

namespace Main
{
    public class NavigationManager
    {
        private readonly Stack<Uri> _visitedPages = new Stack<Uri>();

        public void Navigate(Uri uri)
        {
            _visitedPages.Push(uri);
            // 해당 페이지로 이동하는 로직
        }

        public bool CanGoBack => _visitedPages.Count > 1;

        public Uri GoBack()
        {
            if (_visitedPages.Count > 1)
            {
                _visitedPages.Pop(); // 현재 페이지 제거
                return _visitedPages.Peek(); // 이전 페이지 반환
            }

            return null;
        }
    }
}
