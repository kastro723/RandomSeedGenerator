# RandomSeedGenerator

    [패치 노트]

            v1.1.0 - 24.03.26
                   - Page 기능 추가
                   - Page 당 항목 수 (10,30,50,100) 선택 기능 추가
                   - Seed 사용자 입력 지원
            
            v1.0.0 - 24.03.24
                   - Random Seed Generator 기능 구현

-------------------------------------------------------------------------------------
![image](https://github.com/kastro723/RandomSeedGenerator/assets/55536937/e9a440be-a373-432a-996c-f02ed6db458b)




    [기능설명]
    
            난수 생성 알고리즘(LCG)을 이용한 난수 생성기

            <입력 가능 범위>
                Seed = 0 ~ 9223372036854775807 (long 타입의 양수 범위)
                Min Number, MaxNumber = 0 ~ 2147483647 (int 타입의 양수 범위)
                Iterations = 0 ~ 2147483647 (int 타입의 양수 범위)
                    (Iterations에 매우 큰 수 입력 시 컴퓨터의 메모리 양에 따라 OutOfMemoryException: Out of memory 에러가 일어 날 수 있으니 주의)
            


    [사용방법]
    
            1. 시드(Seed) 값을 입력한다.
            2. 난수 생성 범위(Min Number, Max Number)를 입력한다.
            3. 난수 생성 회수(Iterations)를 입력한다. 
            4. Generate 버튼을 클릭해서 난수를 생성한다.
