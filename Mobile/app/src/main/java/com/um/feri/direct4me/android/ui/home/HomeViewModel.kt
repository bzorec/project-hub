package com.um.feri.direct4me.android.ui.home

import android.widget.Button
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.google.zxing.integration.android.IntentIntegrator
import com.um.feri.direct4me.android.R

class HomeViewModel : ViewModel() {

    private val _text = MutableLiveData<String>().apply {
        value = "For open the box, scan qr code:"
    }

    val text: LiveData<String> = _text
}