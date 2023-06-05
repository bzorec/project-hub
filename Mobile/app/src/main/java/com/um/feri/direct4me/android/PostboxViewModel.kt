package com.um.feri.direct4me.android

import android.content.Context
import androidx.lifecycle.ViewModel

class PostboxViewModel : ViewModel() {
    private val postboxList: MutableList<String> = mutableListOf()
    private var postboxListObserver: ((List<String>) -> Unit)? = null

    fun getPostboxList(): List<String> {
        return postboxList
    }

    fun addPostbox(postboxId: String) {
        postboxList.add(postboxId)
        notifyObserver()
    }

    fun setPostboxList(list: List<String>) {
        postboxList.clear()
        postboxList.addAll(list)
        notifyObserver()
    }

    fun observePostboxList(observer: (List<String>) -> Unit) {
        postboxListObserver = observer
        notifyObserver()
    }

    fun savePostboxList(context: Context) {
        val sharedPreferences =
            context.getSharedPreferences("postbox_preferences", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.putStringSet("postbox_list", postboxList.toSet())
        editor.apply()
    }

    fun loadPostboxList(context: Context) {
        val sharedPreferences =
            context.getSharedPreferences("postbox_preferences", Context.MODE_PRIVATE)
        val savedPostboxList = sharedPreferences.getStringSet("postbox_list", null)
        postboxList.clear()
        if (!savedPostboxList.isNullOrEmpty()) {
            postboxList.addAll(savedPostboxList)
        } else {
            postboxList.add("542") // Add "542" to the list if savedPostboxList is null or empty
        }
        notifyObserver()
    }

    private fun notifyObserver() {
        postboxListObserver?.invoke(postboxList.toList())
    }
}
