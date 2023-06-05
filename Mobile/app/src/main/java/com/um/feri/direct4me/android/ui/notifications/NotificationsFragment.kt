package com.um.feri.direct4me.android.ui.dashboard

import android.os.Bundle
import android.text.TextUtils
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider

import com.um.feri.direct4me.android.R
import com.um.feri.direct4me.android.databinding.FragmentNotificationsBinding


class NotificationsFragment : Fragment() {

    private var _binding: FragmentNotificationsBinding? = null
    private lateinit var titleTextView: TextView
    private lateinit var errorMessageTextView: TextView

    // Initialize ErrorMessage variable
    private var ErrorMessage: String = ""

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentNotificationsBinding.inflate(inflater, container, false)
        val root: View = binding.root

        titleTextView = root.findViewById(R.id.history_title)
        errorMessageTextView = root.findViewById(R.id.error_message)

        // Set error message if applicable
        if (!TextUtils.isEmpty(ErrorMessage)) {
            errorMessageTextView.text = ErrorMessage
            errorMessageTextView.visibility = View.VISIBLE
        }

        return root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}