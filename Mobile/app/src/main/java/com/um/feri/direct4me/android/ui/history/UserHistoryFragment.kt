package com.um.feri.direct4me.android.ui.history

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.um.feri.direct4me.android.R
import com.um.feri.direct4me.android.core.PreferencesHandler

class UserHistoryFragment : Fragment() {
    private lateinit var viewModel: UserHistoryViewModel
    private lateinit var recyclerView: RecyclerView
    private lateinit var adapter: PostboxHistoryAdapter

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_user_history, container, false)

        recyclerView = view.findViewById(R.id.userHistoryRecyclerView)
        recyclerView.layoutManager = LinearLayoutManager(activity)
        adapter = PostboxHistoryAdapter()
        recyclerView.adapter = adapter

        val factory = UserHistoryViewModelFactory(requireContext())
        viewModel = ViewModelProvider(this, factory)[UserHistoryViewModel::class.java]
        viewModel.userHistory.observe(viewLifecycleOwner) { userHistory ->
            adapter.setPostboxHistory(userHistory)
        }
        val user = PreferencesHandler.getInstance().getUserModel()
        if (user != null) {
            viewModel.fetchUserHistory(user.guid)
        }

        return view
    }
}
