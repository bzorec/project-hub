package com.um.feri.direct4me.android.ui.home

import android.content.Intent
import android.media.MediaPlayer
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.google.gson.Gson
import com.google.zxing.integration.android.IntentIntegrator
import com.um.feri.direct4me.android.R
import com.um.feri.direct4me.android.databinding.FragmentHomeBinding
import com.um.feri.direct4me.android.model.OpenBoxRequest
import makeApiRequest
import java.io.OutputStream
import java.net.HttpURLConnection
import java.net.URL

class HomeFragment : Fragment() {

    private var _binding: FragmentHomeBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val homeViewModel =
            ViewModelProvider(this).get(HomeViewModel::class.java)
        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        val root: View = binding.root
        val textView: TextView = binding.textHome
        homeViewModel.text.observe(viewLifecycleOwner) {
            textView.text = it
        }
        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val scanButton: ImageButton = view.findViewById(R.id.scanButton)
        scanButton.setOnClickListener {
            val integrator = IntentIntegrator.forSupportFragment(this)
            integrator.setDesiredBarcodeFormats(IntentIntegrator.QR_CODE)
            integrator.setPrompt("Scan QR Code")
            integrator.setOrientationLocked(true)
            integrator.initiateScan()
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        val result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data)
        try {
            if (result != null && result.contents != null) {
                val boxId = "542"

                // Klic metode OpenBox prek API-ja
                makeApiRequest(boxId.toInt(),"9ea96945-3a37-4638-a5d4-22e89fbc998f")
            }
        }
        catch (e:Exception){

            Log.e("qrError", e.message.toString())
        }
    }

}

