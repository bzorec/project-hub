package com.um.feri.direct4me.android.model

import jakarta.validation.constraints.NotNull
import java.util.*

data class Postbox(
    @field:NotNull val guid: String,
    @field:NotNull val postboxId: Int,
    @field:NotNull val userGuid: String,
    val totalUnlocks: Int,
    @field:NotNull val lastAccess: Date
)