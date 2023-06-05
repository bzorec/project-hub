package com.um.feri.direct4me.android.ui.history

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.um.feri.direct4me.android.R
import java.text.SimpleDateFormat
import java.util.*


class PostboxHistoryAdapter : RecyclerView.Adapter<PostboxHistoryAdapter.ViewHolder>() {
    private val historyList: MutableList<PostboxHistoryItem> = mutableListOf()

    fun setPostboxHistory(history: List<PostboxHistoryItem>) {
        historyList.clear()
        historyList.addAll(history)
        notifyDataSetChanged()
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_history, parent, false)
        return ViewHolder(view)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val historyItem = historyList[position]
        holder.bind(historyItem)
    }

    override fun getItemCount(): Int {
        return historyList.size
    }

    inner class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val dateFormatter = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault())

        private val dateTextView: TextView = itemView.findViewById(R.id.historyDate)
        private val userNameTextView: TextView = itemView.findViewById(R.id.historyUserName)
        private val postboxIdTextView: TextView = itemView.findViewById(R.id.historyPostboxId)
        private val typeTextView: TextView = itemView.findViewById(R.id.historyType)
        private val successTextView: TextView = itemView.findViewById(R.id.historySuccess)

        fun bind(historyItem: PostboxHistoryItem) {
            val formattedDate = dateFormatter.format(historyItem.date)
            dateTextView.text = "Date: $formattedDate"
            userNameTextView.text = "User: ${historyItem.userName}"
            postboxIdTextView.text = "Postbox ID: ${historyItem.postboxId}"
            typeTextView.text = "Type: ${historyItem.type ?: "-"}"
            successTextView.text = "Success: ${historyItem.success}"
        }
    }
}
