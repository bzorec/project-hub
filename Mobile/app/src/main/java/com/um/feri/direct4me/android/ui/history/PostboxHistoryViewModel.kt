package com.um.feri.direct4me.android.ui.history

import android.content.Context
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.um.feri.direct4me.android.api.ApiClient

class PostboxHistoryViewModel(private val context: Context) : ViewModel() {
    val postboxHistory: MutableLiveData<List<PostboxHistoryItem>> = MutableLiveData()

    fun fetchPostboxHistory(boxGuid: String) {
        try {
            ApiClient(context).getPostboxHistory(boxGuid) { items ->
                postboxHistory.postValue(items)
            }
        } catch (e: Exception) {
            print(e.message)
        }
    }
}
