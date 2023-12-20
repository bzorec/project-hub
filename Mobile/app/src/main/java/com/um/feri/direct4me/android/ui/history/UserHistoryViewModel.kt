package com.um.feri.direct4me.android.ui.history

import android.content.Context
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.um.feri.direct4me.android.api.ApiClient
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.util.Date


class UserHistoryViewModel(private val context: Context) : ViewModel() {
    var userHistory: MutableLiveData<List<PostboxHistoryItem>> = MutableLiveData()

    fun fetchUserHistory(userGuid: String) {
        try {
            ApiClient(context).getUserHistory(userGuid) { items ->
                userHistory.postValue(items)
            }
        } catch (e: Exception) {
            print(e.message)
        }
    }
}
