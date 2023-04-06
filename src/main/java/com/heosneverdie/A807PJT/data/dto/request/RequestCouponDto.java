package com.heosneverdie.A807PJT.data.dto.request;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class RequestCouponDto {

    private String nickname;
    private String couponString;
}


